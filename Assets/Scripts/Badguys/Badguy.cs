using UnityEngine;

class Badguy : MonoBehaviour
{
    private Animator _animator;
    [HideInInspector] public float velocity = 0;
    //public float badGuyAccelerationFactor = 1.3f;
    [SerializeField]
    public float totalDistanceTravelled = 0.0f;
    private int _velocityHash;
    [SerializeField]
    private Player _target;
    public float acceleration = 0f;
    private Vector3 endPosition;
    private bool targetCaught = false;
    [SerializeField]
    private SkinnedMeshRenderer mesh;

    [SerializeField] private AudioClip kickSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mesh.enabled = false;
        //acceleration = _target.increasingMaxVelocity * badGuyAccelerationFactor;
        totalDistanceTravelled = -Vector3.Distance(transform.position, _target.transform.position);
        //velocity = _target.velocity;
        _animator = GetComponent<Animator>();
        _velocityHash = Animator.StringToHash("Velocity");

    }

    void Update()
    {
        _animator.SetFloat(_velocityHash, velocity);
        totalDistanceTravelled += velocity * Time.deltaTime;
        if (_target.totalDistanceTravelled < this.totalDistanceTravelled || GameManager.Instance.gameState == GameState.over)
        {
            if (!targetCaught)
            {
                audioSource.clip = kickSound;
                audioSource.Play();

                mesh.enabled = true;
                transform.position = _target.transform.position - _target.transform.forward * 5;
                _target.gotCaught();
                GameManager.Instance.gameState = GameState.over;
                targetCaught = true;
            }

            {
                if (endPosition == Vector3.zero)
                {
                    endPosition = _target.transform.position;
                }
                transform.forward = Vector3.RotateTowards(transform.forward, endPosition - transform.position, 3 * Time.deltaTime, 0.0f);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, 3 * Time.deltaTime);
            }
        }
        //velocity += Time.deltaTime * acceleration;
        velocity = GameManager.Instance.increasingMinVelocity + acceleration;
        //velocity += Time.deltaTime * acceleration;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _animator.SetBool("Kick", true);
            _animator.SetBool("GameOver", true);
        }
    }

}