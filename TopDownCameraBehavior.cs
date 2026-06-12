using UnityEngine;

public class TopDownCameraBehavior : MonoBehavior
{
    public float panSpeed = 20f;
    public float paddingThickness = 20f;
    public float scrollSpeed = 2000f;
    public Vector2 panLimitX = new Vector2(-20, 20);
    public Vector2 panLimitZ = new Vector2(-20, 20);
    public Vector2 zoomLimitY = new Vector2(5, 20);
    bool controlCamera = true;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            controlCamera = !controlCamera;
            
        if(!controlCamera)
            return;
        Vector3 pos = TransformBlock.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - paddingThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= paddingThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - paddingThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x >= paddingThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        //scrolling to zoom in
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * Time.deltaTime * scrollSpeed;
        pos.x = MathF.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = MathF.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        pos.y = MathF.Clamp(pos.y, zoomLimitY.x, zoomLimitY.y);
        transform.position = pos;
    }
}