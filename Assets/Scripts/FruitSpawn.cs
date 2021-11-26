using UnityEngine;

public class FruitSpawn : MonoBehaviour
{
    [SerializeField] private Transform _fruit;
    [SerializeField] private Sprite[] _sprites;

    private AudioSource _audio;
    private float _time = 0;
    private readonly float _spawnTime = 30;
    private readonly float _liveTime = 10;

    public delegate void ScoreDelegate(int value);
    public event ScoreDelegate ScoreChange;

    private void FixedUpdate()
    {
        _audio = GetComponent<AudioSource>();
        _time += Time.deltaTime;
        if(_time > _spawnTime)
        {
            _time = 0;
            Transform fruit = Instantiate(_fruit, transform.position, Quaternion.identity);
            fruit.GetComponent<SpriteRenderer>().sprite = _sprites[new System.Random().Next(0, _sprites.Length)];
            Destroy(fruit.gameObject, _liveTime);
        }
    }

    public void SendScore()
    {
        _audio.Play();
        ScoreChange?.Invoke(1000);
    }
}
