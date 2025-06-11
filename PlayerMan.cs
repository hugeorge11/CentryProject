using UnityEngine;
using System.Collections;

public class PlayerMan : MonoBehaviour {
    // Misc Var
    private Vector3 currVeloMod;
    private Quaternion targetRotation;

    // Config Var 
    [SerializeField, Range(1,10)] private int WALK_SPEED = 6;
    [SerializeField, Range(1,20)] private int RUN_SPEED = 10;
    [SerializeField, Range(5,8)] private int ACC =  6;
    [SerializeField] int ROTATION_SPEED = 550;
    // Modules
    private GameObject Player;
    private GameObject Cam;
    // Controllers
    private CharacterController PC;
    private Camera CC;
    // Transforms
    private Transform PT;
    private Transform CT;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        CC = Cam.GetComponent<Camera>();
        CT = Cam.transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        PC = Player.GetComponent<CharacterController>();
        PT = Player.transform;
    }

    // Update is called once per frame
    void Update(){
        // Movement
        if(PC){
            ControlMouse();
            ControlWASD();
        }
    }
    void ControlMouse(){
        // direction
        if(!Input.GetButton("Sprint")){
            Vector3 mouse = Input.mousePosition;
            mouse = CC.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, CT.position.y - PT.position.y));// handling mouse position
            targetRotation = Quaternion.LookRotation(mouse - new Vector3(PT.position.x, 0, PT.position.z));// rotation we want
            PT.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(PT.eulerAngles.y, targetRotation.eulerAngles.y, ROTATION_SPEED * Time.deltaTime);
        }
    }
    void ControlWASD(){
        Vector3 wasd = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // direction
        if(wasd != Vector3.zero){
            if(Input.GetButton("Sprint")){
                targetRotation = Quaternion.LookRotation(wasd);
                PT.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(PT.eulerAngles.y, targetRotation.eulerAngles.y, ROTATION_SPEED * Time.deltaTime);
            }
        }
        
        // position
        currVeloMod = Vector3.MoveTowards(currVeloMod, wasd, ACC * Time.deltaTime);
        Vector3 motion = currVeloMod;
        motion *= (Mathf.Abs(wasd.x) == 1 && Mathf.Abs (wasd.z) == 1)?.65f:1;
        motion *= (Input.GetButton("Sprint"))?RUN_SPEED:WALK_SPEED;
        motion += Vector3.up * -8;
        PC.Move(motion * Time.deltaTime);// MOVE

    }
}
