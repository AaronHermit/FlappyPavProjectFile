using UnityEngine;

public class Pipes : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public float speed;
    public float gap = 1f;

    private float leftEdge;
    [SerializeField] private Animator _animator;
    Player _player;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        speed = 5f;
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        top.position += Vector3.up * gap / 2;
        bottom.position += Vector3.down * gap / 2;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            Destroy(gameObject,1);
        }
        if (_player.isWaiting)
        {
            speed = 15f;
        }
    }

}
