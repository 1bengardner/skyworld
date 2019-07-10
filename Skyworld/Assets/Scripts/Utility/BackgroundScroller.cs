using UnityEngine;
using System.Collections.Generic;

public class BackgroundScroller : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right, Forward, Backward }
    public float scrollSpeed;
    public float tileSize;
    public Direction direction;
    public bool scrollForever = false;
    Dictionary<Direction, Vector3> directionToVector3 = new Dictionary<Direction, Vector3>()
    {
        { Direction.Up, Vector3.up },
        { Direction.Down, Vector3.down },
        { Direction.Left, Vector3.left },
        { Direction.Right, Vector3.right },
        { Direction.Forward, Vector3.forward },
        { Direction.Backward, Vector3.back },
    };

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = scrollForever ? Time.time * scrollSpeed : Mathf.Repeat(Time.time * scrollSpeed, tileSize);
        transform.position = startPosition + directionToVector3[direction] * newPosition;
    }
}