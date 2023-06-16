using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class Drone : MonoBehaviour
{
    public DRONESTATE _currentState;
    public GameObject _gameObjectToSpawn;
    public float _health;
    public float _maxHealth = 100;
    public GameObject _healthIndicator;
    private NavMeshAgent _myNavMeshAgent;
    public Vector3 _navMeshTarget;
    private GameObject _playerObject;
    private float _t; // timer for changing _navMeshTarget
    public float _refreshRate = 2.5f;
    public float _distanceToPlayer;
    public float _chaseRange = 10f;
    public float _attackRange = 6f;
    public float _playerSpeed = 3f;
    public GameObject _bullet;
    public GameObject _bomb;
    public float _fireRate = .3f; // a
    public float _bombFireRate = 1f;
    public float _bulletVelocity = 3f; // b
    public int _bombAmount = 20;
    public float _bombVelocity = 3f;
    private bool _canFire = true;
    private bool _canFireBomb = true;
    public float _damageAmount = 10f;
    public string _opposingTeamTag;
    public Vector3 _aimPosition; // c
    public Vector2 _regenerationRateAmount = new Vector2(1f,5f);
    public GameObject _droneParticle;
    public int _numberOfParticles = 12;
    public LayerMask _layerMask;
	private Vector3 _direction;

    void Start()
    {
        _health = _maxHealth;
        _myNavMeshAgent = GetComponent<NavMeshAgent>();
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        //_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //_mainCamera.GetComponent<GameManager>().IncreaseNumberOfDrones();
        _t = _refreshRate;
        StartCoroutine(RegenerateHealthPlease(_regenerationRateAmount.x, _regenerationRateAmount.y));
    }

    void LateUpdate()
    {
        _healthIndicator.transform.localScale = new Vector3(_health/_maxHealth,_health/_maxHealth,_health/_maxHealth);
        _distanceToPlayer = Vector3.Distance(transform.position , _playerObject.transform.position);
        _t -= Time.deltaTime;
        
        if (_health < 1f) Die();

        switch (_currentState)
        {
            case DRONESTATE.Wander:
            {
                if (_distanceToPlayer > _chaseRange)
                {
                    if (_t <= 0.0f) 
                    {
                        setRandomPosition();
                        _t = _refreshRate;
                    }
                }
                else _currentState = DRONESTATE.Chase;
                break;
            }

            case DRONESTATE.Chase:
            {
                _navMeshTarget = _playerObject.transform.position;
                
                if (_distanceToPlayer > _chaseRange)
                {
                    _currentState = DRONESTATE.Wander;
                }
                if (_distanceToPlayer < _attackRange)
                {
                    _currentState = DRONESTATE.Attack;
                }
				
                break;
            }

            case DRONESTATE.Attack:
            {
                _navMeshTarget = _playerObject.transform.position;
				_direction = Vector3.Normalize(_navMeshTarget - transform.position);
				
                if (_distanceToPlayer > _attackRange)
                {
                    _currentState = DRONESTATE.Chase;
                }
                else
                {
					var rayColor = CanHitPlayer() ? Color.green : Color.red;
                	Debug.DrawRay(transform.position, _direction * _attackRange, rayColor);
                    if (CanHitPlayer() &&_canFire) 
                    {
                        SetTargetPosition();
						StartCoroutine(FireBulletPlease());
                    }
                }
                break;
            }

            case DRONESTATE.Spawn:
            {
                if (_distanceToPlayer < _attackRange && _canFire)
                {
                    StartCoroutine(SpawnObjectPlease(_gameObjectToSpawn));
                }
                else
                {
                    if (_t <= 0.0f) 
                    {
                        setRandomPosition();
                        _t = _refreshRate;
                    }
                }
                break;
            }

            case DRONESTATE.Player:
            {
                if (Input.GetMouseButton(0))
                {
                SetTargetPosition();
                    if (_canFire)
                    {
                        StartCoroutine(FireBulletPlease());
                    }
                }
                if (Input.GetMouseButton(1))
                {
                SetTargetPosition();
                    if (_canFireBomb)
                    {
                        StartCoroutine(FireBombPlease());
                    }
                }
                break;
            }
        }
        if (_currentState != DRONESTATE.Player && _navMeshTarget != null) _myNavMeshAgent.SetDestination(_navMeshTarget);
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
        if (_currentState != DRONESTATE.Player) 
        {
            Destroy(gameObject);
            //_mainCamera.GetComponent<GameManager>().DecreaseNumberOfDrones();
            //_mainCamera.GetComponent<GameManager>().IncreaseScore(1);
            for (int x = 0; x < _numberOfParticles; x = x + 1)
            {
                GameObject g = Instantiate(_droneParticle, transform.position, Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3,3),
                Random.Range(-3,3),Random.Range(-3,3)) * Random.Range(1,2), ForceMode.Impulse);
            }
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>().LoadThisLevel();
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
        if (_currentState != DRONESTATE.Player)
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
        if (collision.collider.tag == _opposingTeamTag)
        {
        TakeDamage(_damageAmount);
		}
    }

    IEnumerator FireBulletPlease()
    {
        _canFire = false;
        transform.LookAt(_aimPosition);
        GameObject g = Instantiate(_bullet, transform.position, Quaternion.identity);
        g.GetComponent<Collider>().isTrigger = true;
        g.transform.LookAt(_aimPosition);
        g.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * _bulletVelocity, ForceMode.Impulse);
        yield return new WaitForSeconds(0.05f);
        g.GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
    }

    IEnumerator FireBombPlease()
    {
        // _canFireBomb = false;
        // for (int x = 0; x < 20; x = x + 1)
        // {
        //     transform.Rotate(transform.rotation.x, transform.rotation.y + x, transform.rotation.z);
            
        //     GameObject g = Instantiate(_bullet, transform.position, Quaternion.identity);
        //     g.GetComponent<Collider>().isTrigger = true;
            
        //     g.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * _bulletVelocity, ForceMode.Impulse);
        //     yield return new WaitForSeconds(0.05f);
        //     g.GetComponent<Collider>().isTrigger = false;
        //     yield return new WaitForSeconds(_fireRate / 8);
        //     _canFireBomb = true;
        // }
        // yield return new WaitForSeconds(_bombFireRate);
        _canFireBomb = false;
        for (int x = 0; x < _bombAmount; x = x + 1)
            {
                GameObject g = Instantiate(_bullet, transform.position, Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10,10),0f,Random.Range(-10,10)) * _bombVelocity, ForceMode.Impulse);
            }
        yield return new WaitForSeconds(_bombFireRate);
        _canFireBomb = true;
    }
    
    IEnumerator RegenerateHealthPlease(float _regenRate, float _regenAmount)
    {
        yield return new WaitForSeconds(_regenRate);
        if (_health < _maxHealth) _health += _regenAmount;
        
        StartCoroutine(RegenerateHealthPlease(_regenRate,_regenAmount));
    }

    IEnumerator SpawnObjectPlease(GameObject g)
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireRate);
        Instantiate
            (g,
            new Vector3(Random.Range(
            transform.position.x - 5f,transform.position.x + 5f),
            transform.position.y,
            Random.Range(transform.position.z - 5,transform.position.z + 5)
            ), 
            Quaternion.identity
            );
        
        _canFire = true;
        //StartCoroutine(SpawnObjectPlease(g));
    }
}
public enum DRONESTATE
{
    Wander,
    Chase,
    Attack,
    Player,
    Spawn
}