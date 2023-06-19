using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class Sentient : MonoBehaviour
{
    public SENTIENTSTATE _currentState;
    public float _health;
    public float _maxHealth = 100;
    public GameObject _healthIndicator;
    private NavMeshAgent _myNavMeshAgent;
    public Vector3 _navMeshTarget;
    private GameObject _playerObject;
    private float _t;
    public float _refreshRate = 2.5f;
    public float _distanceToPlayer;
    public float _chaseRange = 10f;
    public float _attackRange = 6f;
    public float _playerSpeed = 3f;
    public GameObject _bullet;
    public float _primaryFireRate = .3f;
    public float _secondaryFireRate = 1f;
    public float _primaryVelocity = 3f;
    public int _secondaryAmount = 4;
    public float _secondaryVelocity = 3f;
    private bool _canFire = true;
    private bool _canFireSecondary = true;
    public float _damageAmount = 10f;
    public string _opposingTeamTag;
    public Vector3 _aimPosition;
    public Vector2 _regenerationRateAmount = new Vector2(1f,5f);
    public GameObject _sentientParticle;
    public int _numberOfParticles = 12;
    public LayerMask _layerMask;
	private Vector3 _direction;

    void Start()
    {
        _health = _maxHealth;
        _myNavMeshAgent = GetComponent<NavMeshAgent>();
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _t = _refreshRate;
        StartCoroutine(RegenerateHealthPlease(_regenerationRateAmount.x, _regenerationRateAmount.y));
    }

    void LateUpdate()
    {
        _healthIndicator.transform.localScale = new Vector3(_health/_maxHealth,_health/_maxHealth,_health/_maxHealth);
        _distanceToPlayer = Vector3.Distance(transform.position , _playerObject.transform.position);
        _t -= Time.deltaTime;
        
        if (_health < 0) Die();

        switch (_currentState)
        {
            case SENTIENTSTATE.Wander:
            {
                if (_distanceToPlayer > _chaseRange)
                {
                    if (_t <= 0.0f) 
                    {
                        setRandomPosition();
                        _t = _refreshRate;
                    }
                }
                else _currentState = SENTIENTSTATE.Chase;
                break;
            }

            case SENTIENTSTATE.Chase:
            {
                _navMeshTarget = _playerObject.transform.position;
                
                if (_distanceToPlayer > _chaseRange)
                {
                    _currentState = SENTIENTSTATE.Wander;
                }
                if (_distanceToPlayer < _attackRange)
                {
                    _currentState = SENTIENTSTATE.Attack;
                }
				
                break;
            }

            case SENTIENTSTATE.Attack:
            {
                _navMeshTarget = _playerObject.transform.position;
				_aimPosition = _playerObject.transform.position;
				_direction = Vector3.Normalize(_aimPosition - transform.position);
				
                if (_distanceToPlayer > _attackRange)
                {
                    _currentState = SENTIENTSTATE.Chase;
                }
                else
                {
					var rayColor = CanHitPlayer() ? Color.green : Color.red;
                	Debug.DrawRay(transform.position, _direction * _attackRange, rayColor);
                    if (CanHitPlayer() &&_canFire) 
                    {
						if (_health / _maxHealth <= 0.5f && _canFireSecondary)
						{
							SetTargetPosition();
							StartCoroutine(FireSecondaryPlease());
						}
						else
						{
							SetTargetPosition();
							StartCoroutine(FirePrimaryPlease());
						}
                    }
                }
                break;
            }

            case SENTIENTSTATE.Player:
            {
                if (Input.GetMouseButton(0))
                {
                SetTargetPosition();
                    if (_canFire)
                    {
                        StartCoroutine(FirePrimaryPlease());
                    }
                }
                if (Input.GetMouseButton(1))
                {
                SetTargetPosition();
                    if (_canFireSecondary)
                    {
                        StartCoroutine(FireSecondaryPlease());
                    }
                }
                break;
            }
        }
        if (_currentState != SENTIENTSTATE.Player && _navMeshTarget != null) _myNavMeshAgent.SetDestination(_navMeshTarget);
        if (_navMeshTarget == null) setRandomPosition();
    }

	private bool CanHitPlayer()
    {
		bool val = false;
		RaycastHit hit;
        if (Physics.Raycast(transform.position, _direction, out hit, _attackRange, _layerMask))
		{
			if (hit.collider.gameObject.CompareTag("Player"))
			{
				val = true;
			}
		}
		return val;
    }
                 

    public void TakeDamage(float d)
    {
        _health -= d;
    }
    public void Die()
    {
        if (_currentState != SENTIENTSTATE.Player) 
        {
            Vector3 pos = transform.position;
			
			for (int x = 0; x < _numberOfParticles; x = x + 1)
            {
                GameObject g = Instantiate(_sentientParticle, pos, Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3,3),
                Random.Range(-3,3),Random.Range(-3,3)) * Random.Range(1,2), ForceMode.Impulse);
            }
			Destroy(gameObject);
        }
		else
		{
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>().LoadThisLevel();
		}
    }

    private void setRandomPosition()
    {
        _navMeshTarget = new Vector3(
            Random.Range(transform.position.x - 5,transform.position.x + 5), 
            transform.position.y, 
            Random.Range(transform.position.z - 5,transform.position.z + 5));
    }

    private void SetTargetPosition()
    {
        if (_currentState != SENTIENTSTATE.Player)
        {
            _aimPosition = _playerObject.transform.position;
        }
        else
        {
            RaycastHit _hitInfo;
            Vector2 _mousePosition = Input.mousePosition;
            Ray _rayOrigin = Camera.main.ScreenPointToRay(_mousePosition);
            if (Physics.Raycast(_rayOrigin, out _hitInfo))
            {
                _aimPosition = new Vector3(_hitInfo.point.x, transform.position.y, _hitInfo.point.z );
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == _opposingTeamTag || collision.collider.tag == "Player" )
        {
        TakeDamage(_damageAmount);
		}
		
		if (_currentState == SENTIENTSTATE.Player)
		{
			if (collision.collider.tag == "Enemy")
			{
				TakeDamage(_damageAmount);
			}

			if (collision.collider.tag == "PowerSphere")
			{
				Destroy(collision.gameObject);
				if (_health < _maxHealth)TakeDamage(-20);
			}
		}
    }

    IEnumerator FirePrimaryPlease()
    {
        _canFire = false;
        transform.LookAt(_aimPosition);
        GameObject g = Instantiate(_bullet, transform.position, Quaternion.identity);
        g.GetComponent<Collider>().isTrigger = true;
        g.transform.LookAt(_aimPosition);
        g.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * _primaryVelocity, ForceMode.Impulse);
        yield return new WaitForSeconds(.005f);
        g.GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(_primaryFireRate);
        _canFire = true;
    }

    IEnumerator FireSecondaryPlease()
    {
        _canFireSecondary = false;
		for (int i = 0; i < _secondaryAmount; i++)
		{
		    transform.LookAt(GetRandomPositionNear(_aimPosition, 2));
		    GameObject g = Instantiate(_bullet,transform.position, Quaternion.identity);
		    g.GetComponent<Collider>().isTrigger = true;
		    g.transform.LookAt(_aimPosition);
		    g.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * _secondaryVelocity, ForceMode.Impulse);
        	yield return new WaitForSeconds(0.05f);
        	g.GetComponent<Collider>().isTrigger = false;
		}
        yield return new WaitForSeconds(_secondaryFireRate);
        _canFireSecondary = true;
    }

	private Vector3 GetRandomPositionNear(Vector3 position , int radius)
    {
    	return new Vector3(Random.Range(position.x - radius, position.x + radius), transform.position.y, Random.Range(position.z - radius, position.z + radius));
    }
    
    IEnumerator RegenerateHealthPlease(float _regenRate, float _regenAmount)
    {
        yield return new WaitForSeconds(_regenRate);
        if (_health < _maxHealth) _health += _regenAmount;
        
        StartCoroutine(RegenerateHealthPlease(_regenRate,_regenAmount));
    }
}
public enum SENTIENTSTATE
{
    Wander,
    Chase,
    Attack,
    Player,
}