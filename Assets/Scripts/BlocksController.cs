using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BlocksController : MonoBehaviour
{
    public List<Material> materials;
    public GameObject basicPrefab;

    [SerializeField] private int x;
    [SerializeField] private int y;

    private readonly FileAccessor _accessor = new FileAccessor();

    private Renderer[,] _blocks;
    private string _path = "";
    private bool _updateFlag;
    
    private const int Padding = 2;

    // Start is called before the first frame update
    private void Start()
    {
        _blocks = new Renderer[3, 3];
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            var go = Instantiate(basicPrefab, new Vector3((-1 + i) * -1.5f, 0, (-1 + j) * 1.5f), Quaternion.identity,
                transform);
            _blocks[j, i] = go.GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var (rows, cols) = _accessor.Size;

        //Inputs section (i dont like it but importing new input system is too much)
        if (Input.GetKeyDown(KeyCode.W))
        {
            x = Mathf.Clamp(x - 1, 1-Padding, rows+2-Padding);
            _updateFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            x = Mathf.Clamp(x + 1, 1-Padding, rows+2-Padding);
            _updateFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            y = Mathf.Clamp(y - 1, 1-Padding, cols+2-Padding);
            _updateFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            y = Mathf.Clamp(y + 1, 1-Padding, cols+2-Padding);
            _updateFlag = true;
        }

        // Blocks update section

        if (!_updateFlag) return;
        _updateFlag = false;

        var matrix = _accessor[x, y];
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            if (matrix[i, j] != -1)
            {
                _blocks[i, j].enabled = true;
                _blocks[i, j].material = materials[matrix[i, j] - 1];
            }
            else
            {
                _blocks[i, j].enabled = false;
            }
    }

    public void UpdatePath(string path)
    {
        _path = path;
    }

    public void Read()
    {
        _accessor.Open(_path).ContinueWith(task =>
            {
                _updateFlag = task.Result;

                if (!task.Result) return; //the following code is executed only when numbers are successfully loaded

                var (rows, cols) = _accessor.Size;
                var rnd = new Random();
                x = rnd.Next(1-Padding, rows+2-Padding);
                y = rnd.Next(1-Padding, cols+2-Padding);
            }
        );
    }
}