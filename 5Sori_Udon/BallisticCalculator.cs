
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class BallisticCalculator : UdonSharpBehaviour
{
    //public LineRenderer lr;
    public Rigidbody VehicleRigid; 
    public Transform FirePose;
    public int NumberOfPredictPoints=30;
    public float interval=1.0f;
    public GameObject ccipHud;
    public Camera AtgCam;
    public LayerMask BlockableLayer = (1<<0)|(1<<4)|(1<<11);
    public GameObject ActiveIsActive;
    public Text impactTimeText;
    public float muzzleVelocity = 0f;
    public float mass=1f;
    //wip
    [Header("Experimental Functions"),Tooltip("Experimental function, currently not functioning properly, only projectiles with zero air resistance can be calculated")]
    public bool CalculateDrag=false;
    public float drag = 0f;
    [Header("Debug Functions"),Tooltip("Draw a parabolic curve predicting trajectory in the scene")]
    public bool debugRay=true;
    //===============================
    private float impactTime=0;
    private bool bombLaunched=false;
    private Vector3 startPosition;
    private Vector3 startVelocity;
    private RaycastHit hit;
    private Vector3 impactPoint;
    //private Vector3 AirphysicsForce;
    int i=0;

    // Start is called before the first frame update
    void Start()
    {
        impactTimeText.text="UNABLE";
    }

    // Update is called once per frame
    void Update()
    {
      if(ActiveIsActive.activeSelf)
      {
        drawline();
      }
      else{
        impactTimeText.text="UNABLE";
        ccipHud.SetActive(false);
      }

        
    }
    public void SFEXT_O_BombLaunch(){
      Debug.Log("폭탄투하");
      bombLaunched=true;
    }
    private void drawline()
    {   //초기화
        AtgCam.fieldOfView =10f;
        i=0; 
        startPosition=FirePose.position;
        startVelocity=VehicleRigid.velocity;
        //
        if(muzzleVelocity!=0){//발사형이라면 시작속도에 발사속도추가
        startVelocity+=FirePose.forward*muzzleVelocity;
        }


        Vector3 PrevPose=startPosition;
        Vector3 currentVelocity=startVelocity/mass;
        
        for(float j=0;i<NumberOfPredictPoints-1;j+=interval)
        {
            Vector3 NextPose=PrevPose+startVelocity*interval;
            i++;
            if(CalculateDrag==true){//experimental
              currentVelocity+=Physics.gravity*interval;
              currentVelocity*=(1-drag*interval);
              NextPose=currentVelocity*interval+PrevPose;
            }
            else{
              NextPose=startPosition+j*currentVelocity;
              NextPose.y=startPosition.y+currentVelocity.y*j+0.5f*Physics.gravity.y*j*j; //자유낙하 탄도 공기저항X
            }
            if(debugRay){ //디버그용 포물선 선보이기
              Debug.DrawRay(PrevPose,NextPose-PrevPose,Color.white,0.05f);
            }
            

            if(Physics.Raycast(PrevPose,NextPose-PrevPose,out hit,Vector3.Distance(NextPose,PrevPose),BlockableLayer)) //레이케스트가 충돌시 
            {
                impactPoint=hit.point;
                ccipHud.SetActive(true);
                ccipHud.transform.LookAt(impactPoint,Vector3.up);
                AtgCam.transform.LookAt(impactPoint,Vector3.up);
                if(i<NumberOfPredictPoints-1&&bombLaunched==false){ //착탄시간표시
                  impactTime = (i)*interval;
                  impactTimeText.text=impactTime.ToString("F1")+"S";
                }
                else if(i>=NumberOfPredictPoints-1&&bombLaunched==false){
                  impactTimeText.text="UNABLE";
                }
                break;
            }
            else{
                ccipHud.SetActive(false);
                PrevPose=NextPose;
            }
        }
        if(bombLaunched==true)
        {
          impactTime-=Time.deltaTime;
          impactTimeText.text=impactTime.ToString("F1")+"T";
          if(impactTime<0){
            bombLaunched=false;
          }
        }
        

    }
}
