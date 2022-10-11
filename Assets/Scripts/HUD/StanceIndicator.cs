using Unity;
using UnityEngine;
using UnityEngine.UI;
class StanceIndicator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private GameObject _stance;
    [SerializeField]
    private GameObject _stanceDuration;
    private float _stanceTime = 0.0f;
    private float duration = 0.0f;
    private bool _isIdle = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _stance.transform.eulerAngles = new Vector3(0, 90, 0);

    }
    void Update()
    {

        if (!_isIdle)
        {
            duration -= Time.deltaTime;
            updateStanceDuration();

        }
        else
        {
            this._stanceDuration.transform.localScale = Vector3.zero;
        }


    }
    void updateStanceDuration()
    {
        print("update stance duration");
        float newWidth = duration / _stanceTime;
        if (newWidth <= .2f)
        {

            _stanceDuration.GetComponent<Image>().material.color = Color.red;
        }
        else
        {
            _stanceDuration.GetComponent<Image>().material.color = Color.black;
        }
        _stanceDuration.transform.localScale = new Vector3(duration / _stanceTime, 1f, 1f);


    }
    public void setIdle()
    {
        _isIdle = true;
        _animator.SetBool("medium", false);
        _animator.SetBool("low", false);
        _animator.SetBool("high", false);
        _stance.transform.eulerAngles = new Vector3(0, 90, 0);

    }
    public void setMedium(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;

        _isIdle = false;
        _animator.SetBool("medium", true);
        _animator.SetBool("low", false);
        _animator.SetBool("high", false);
        _stance.transform.eulerAngles = new Vector3(0, 130, 0);

    }
    public void setLow(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;
        _isIdle = false;
        _animator.SetBool("medium", false);
        _animator.SetBool("low", true);
        _animator.SetBool("high", false);
        _stance.transform.eulerAngles = new Vector3(0, 90, 0);


    }
    public void setHigh(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;

        _isIdle = false;
        _animator.SetBool("medium", false);
        _animator.SetBool("low", false);
        _animator.SetBool("high", true);
        _stance.transform.eulerAngles = new Vector3(0, 130, 0);
    }

}