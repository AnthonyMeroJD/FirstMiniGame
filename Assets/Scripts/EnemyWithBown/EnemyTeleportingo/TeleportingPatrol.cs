using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportingPatrol : MonoBehaviour
{

    enum Direccion { UP,DOWN,LEFT,RIGHT}
    enum Alley {CORNERTOPLEFT,CORNERTOPRIGHT,CORNERBOTLEFT,CORNERBOTRIGHT,ALLEYTOP,ALLEYBOT,ALLEYRIGHT,ALLEYLEFT,NOALLEY }
    
    //
    public float maxX ,maxY;
    public Vector3 target;
    //stats
    public float health, maxHealth, rageAtack,teleportZoneRage,visionRage,speedAtack,distanceToTp,teleportCooldown;
    //statuss
    private bool isEnemyToAtackRage;
    private bool isAtacking;
    private bool isEnemyIntoTpZone,tpBefore;
    //components
    //private VisionRage visionRage;
    private Animator animator;
    private Rigidbody2D rigidbody;
    //Gameobjects
    GameObject player;


    float distanceToPlayer;
    RaycastHit2D hit;
    Vector3 dirToPlayer;
    void initializateComponent() {
        rigidbody = GetComponent<Rigidbody2D>();
        //visionRage = GetComponentInChildren<VisionRage>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Awake() {
        initializateComponent();
    }
    void Start()
    {
        tpBefore = false;
        isAtacking = false;
    }

    void Update()
    {
        TrigerRaycast(transform.position,DirToPlayer(),visionRage,"Default",true,Color.red);
        IsEnemyToAtackRage();
        IsEnemyIntoTpZone();
    }
    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        if (isEnemyToAtackRage) Flip();
        if (isEnemyToAtackRage && !isAtacking  ) StartCoroutine( "Atack");
        if (isEnemyIntoTpZone && !tpBefore )  StartCoroutine("Teleport") ;
        
    }

    float DistanceToPlayer()
    {
        return distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
    }

    Vector3 DirToPlayer()
    {
        return dirToPlayer = player.transform.position - transform.position;
    }

    RaycastHit2D TrigerRaycast(Vector3 init, Vector3 direccion, float alcance, string layer)
    {
        return hit = Physics2D.Raycast(init, direccion, alcance,
               1 << LayerMask.NameToLayer(layer));

    }

    RaycastHit2D TrigerRaycast(Vector3 init, Vector3 direccion, float alcance, string layer, bool debug, Color color)
    {
        if (debug) DebugRaycast(init, direccion, color);
        return hit = Physics2D.Raycast(init, direccion, alcance,
               1 << LayerMask.NameToLayer(layer));

    }

    void DebugRaycast(Vector3 init, Vector3 direccion, Color color)
    {
        Debug.DrawRay(init, direccion, color);
    }

    void IsEnemyToAtackRage()
    {
        isEnemyToAtackRage = DistanceToPlayer() < rageAtack;
    }

    void IsEnemyIntoTpZone()
    {
        isEnemyIntoTpZone = DistanceToPlayer() < teleportZoneRage;
    }

    IEnumerator Teleport() {
        tpBefore = true;
        animator.Play("Teleport");
        yield return new WaitForSeconds(teleportCooldown);
        tpBefore = false;
    }

    IEnumerator Atack() {
        
        isAtacking = true;
        animator.Play("Atack");       
        yield return new WaitForSeconds(speedAtack);
        isAtacking = false;       
    }

    void Flip()
    { 
        if (dirToPlayer.normalized.x > 0) transform.GetComponent<SpriteRenderer>().flipX = false;
        else transform.GetComponent<SpriteRenderer>().flipX = true;
    }

    bool isPlayerToRight() {

        return dirToPlayer.normalized.x > 0;
    }

    bool IsThereWallWhereGo( Vector3 direccion) {
        RaycastHit2D hitR= TrigerRaycast(transform.position, direccion, teleportZoneRage, "Default", true, Color.blue);
        if (hitR.collider != null && hitR.collider.tag == "Walls") {
            return true;
        }
        return false;
    }

    Dictionary<Direccion,bool> AreThereWallsWhereGo()
    {
        Dictionary<Direccion, bool> isWallsHere=new Dictionary<Direccion, bool>();
        isWallsHere.Add(Direccion.RIGHT, IsThereWallWhereGo(Vector2.right));
        isWallsHere.Add(Direccion.LEFT, IsThereWallWhereGo(Vector2.left));
        isWallsHere.Add(Direccion.UP, IsThereWallWhereGo(Vector2.up));
        isWallsHere.Add(Direccion.DOWN, IsThereWallWhereGo(Vector2.down));
        return isWallsHere;



    }
    int WhereSectorIsPlayerInTpArea() {
        float x, y;
        int sector = 0;
        x = DirToPlayer().normalized.x;
        y = DirToPlayer().normalized.y;

        if (x > 0 && y > 0) sector = 1;
        if (x < 0 && y > 0) sector = 2;
        if (x < 0 && y < 0) sector = 3;
        if (x > 0 && y < 0) sector = 4;

        return sector;
    }
    bool CanTeleportTo(Direccion direccion) {
        Dictionary<Direccion, bool> areWallsHere = AreThereWallsWhereGo();
        bool can = !areWallsHere[direccion];       
        return can;
    }

    bool IsThisAlley(Alley alley) {
        bool isAlley = false;
        switch (alley) {
            case Alley.ALLEYTOP:
                isAlley= !CanTeleportTo(Direccion.LEFT) && !CanTeleportTo(Direccion.RIGHT) && 
                         !CanTeleportTo(Direccion.UP) && CanTeleportTo(Direccion.DOWN);
                break;
            case Alley.ALLEYBOT:
                isAlley= !CanTeleportTo(Direccion.LEFT) && !CanTeleportTo(Direccion.RIGHT) && 
                         !CanTeleportTo(Direccion.DOWN) && CanTeleportTo(Direccion.UP);
                break;
            case Alley.ALLEYRIGHT:
                isAlley = !CanTeleportTo(Direccion.RIGHT) && !CanTeleportTo(Direccion.UP) && 
                          !CanTeleportTo(Direccion.DOWN) && CanTeleportTo(Direccion.LEFT);
                break;
            case Alley.ALLEYLEFT:
                isAlley = !CanTeleportTo(Direccion.LEFT) && !CanTeleportTo(Direccion.UP) && 
                          !CanTeleportTo(Direccion.DOWN) && CanTeleportTo(Direccion.RIGHT);
                break;
            case Alley.CORNERBOTLEFT:
                isAlley= !CanTeleportTo(Direccion.LEFT) && !CanTeleportTo(Direccion.DOWN) && 
                          CanTeleportTo(Direccion.UP) && CanTeleportTo(Direccion.RIGHT);
                break;
            case Alley.CORNERBOTRIGHT:
                isAlley = !CanTeleportTo(Direccion.RIGHT) && !CanTeleportTo(Direccion.DOWN )&&
                            CanTeleportTo(Direccion.UP) && CanTeleportTo(Direccion.LEFT);
                break;
            case Alley.CORNERTOPLEFT:
                isAlley = !CanTeleportTo(Direccion.LEFT) && !CanTeleportTo(Direccion.UP) && 
                            CanTeleportTo(Direccion.DOWN) && CanTeleportTo(Direccion.RIGHT);
                break;
            case Alley.CORNERTOPRIGHT:
                isAlley = !CanTeleportTo(Direccion.RIGHT) && !CanTeleportTo(Direccion.UP) &&
                            CanTeleportTo(Direccion.LEFT) && CanTeleportTo(Direccion.DOWN); 
                break;
                
        }
        return isAlley;
    }    
    
    Dictionary<Alley,bool> GetsAlleysImHere() {
        Dictionary<Alley, bool> alley = new Dictionary<Alley, bool>();
        alley.Add(Alley.CORNERBOTRIGHT, IsThisAlley(Alley.CORNERBOTRIGHT));
        alley.Add(Alley.CORNERTOPRIGHT, IsThisAlley(Alley.CORNERTOPRIGHT));
        alley.Add(Alley.CORNERBOTLEFT, IsThisAlley(Alley.CORNERBOTLEFT));
        alley.Add(Alley.CORNERTOPLEFT, IsThisAlley(Alley.CORNERTOPLEFT));
        alley.Add(Alley.ALLEYTOP, IsThisAlley(Alley.ALLEYTOP));
        alley.Add(Alley.ALLEYBOT, IsThisAlley(Alley.ALLEYBOT));
        alley.Add(Alley.ALLEYRIGHT, IsThisAlley(Alley.ALLEYRIGHT));
        alley.Add(Alley.ALLEYLEFT, IsThisAlley(Alley.ALLEYLEFT));
        return alley;
    }
    Alley GetAlleyInHere() {
        Alley alley = Alley.NOALLEY;
        foreach(KeyValuePair<Alley,bool> pair in GetsAlleysImHere())
        {
            print("values:" + pair.Value + "/" + pair.Key);
            if (pair.Value) alley= pair.Key;
        }
        return alley;
    }
    bool AmICornered() {
        return GetsAlleysImHere().ContainsValue(true);                  
    }

    Vector3 GetTeleportDireccion(Direccion direccion)
    { 
            Vector3 target=Vector3.zero;
        switch (direccion) {
            case Direccion.UP:
          target=     new Vector3(0, distanceToTp);
                break;
            case Direccion.DOWN:
                target = new Vector3(0, distanceToTp*-1);
                break;
            case Direccion.LEFT:
                target = new Vector3(distanceToTp * -1, 0);
                break;
            case Direccion.RIGHT:
                target = new Vector3(distanceToTp , 0);
                break;
        }
        return target;
        
    }
    Vector3 DecidetargetToTp() {
        Vector3 target = Vector3.zero;
        switch (GetAlleyInHere()) {
            case Alley.ALLEYBOT:
                target = GetTeleportDireccion(Direccion.UP);
                break;
            case Alley.ALLEYTOP:
                target = GetTeleportDireccion(Direccion.DOWN);
                break;
            case Alley.ALLEYRIGHT:
                target = GetTeleportDireccion(Direccion.LEFT);
                break;
            case Alley.ALLEYLEFT:
                target = GetTeleportDireccion(Direccion.RIGHT);
                break;
            case Alley.CORNERBOTLEFT:             
                if(WhereSectorIsPlayerInTpArea()<=2) target = GetTeleportDireccion(Direccion.RIGHT) ;
                else target = GetTeleportDireccion(Direccion.UP); ;                
                break;
            case Alley.CORNERBOTRIGHT:
                if (WhereSectorIsPlayerInTpArea() <= 2) target = GetTeleportDireccion(Direccion.LEFT); 
                else target = GetTeleportDireccion(Direccion.UP); ;
                break;
            case Alley.CORNERTOPLEFT:
                if (WhereSectorIsPlayerInTpArea() == 1 || WhereSectorIsPlayerInTpArea() == 4) 
                    target = GetTeleportDireccion(Direccion.DOWN); 
                else  target = GetTeleportDireccion(Direccion.RIGHT); 
                break;
            case Alley.CORNERTOPRIGHT:
                if (WhereSectorIsPlayerInTpArea() == 2 || WhereSectorIsPlayerInTpArea() == 3)
                    target = GetTeleportDireccion(Direccion.DOWN);
                else target = GetTeleportDireccion(Direccion.LEFT);
                break;
        }
        return target;
    }
    void PatronTeleporteWhenCornered() {
      transform.position+= DecidetargetToTp();
            
    }

    bool AreTheyWallsInAround()
    {
        bool imAroundWithWalls=true;
        foreach (KeyValuePair<Direccion, bool> pair in WallsAroundMe()) {
            if (pair.Value.Equals(false)) imAroundWithWalls = false;
            
        }
        return true; 

    }
    Dictionary<Direccion, bool> WallsAroundMe() {
        Dictionary<Direccion, bool> wallsAroundMe= new Dictionary<Direccion, bool>();
        wallsAroundMe.Add(Direccion.UP, !CanTeleportTo(Direccion.UP));
        wallsAroundMe.Add(Direccion.DOWN, !CanTeleportTo(Direccion.DOWN));
        wallsAroundMe.Add(Direccion.LEFT, !CanTeleportTo(Direccion.LEFT));
        wallsAroundMe.Add(Direccion.RIGHT, !CanTeleportTo(Direccion.RIGHT));
        return wallsAroundMe;    

    }
    Vector3 GetTeleportDireccionOpositePlayer() {
        int sector = WhereSectorIsPlayerInTpArea();
        Vector3 target = Vector3.zero;
        if (sector == 2 || sector == 3) target = GetTeleportDireccion(Direccion.RIGHT);
        else target=GetTeleportDireccion(Direccion.LEFT);
        return target;
    }
    List< Direccion> GetWallsOnlyAroundMe() {
        List<Direccion> direccions=new List<Direccion>();
        foreach (KeyValuePair<Direccion, bool> pair in WallsAroundMe())
        {          
            if (pair.Value.Equals(true)) direccions.Add(pair.Key);

        }
        return direccions;
    }
    void PatrolTeleportIfWallsAroundAndNotCornered()
    {
        Direccion direccion=Direccion.DOWN;
        GetWallsOnlyAroundMe();

        if( transform.position.x<maxX)
        transform.position += GetTeleportDireccion(direccion);

    }

    void PatronTeleportFree() {
        if (!AreTheyWallsInAround()) transform.position +=GetTeleportDireccionOpositePlayer();
       else PatrolTeleportIfWallsAroundAndNotCornered();
       
    }
    void TeleportOut() {
        if (AmICornered()) PatronTeleporteWhenCornered();
        else PatronTeleportFree();      
        animator.Play("TelportIn");


    }   

    private void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        print(pos);
        GUI.Box(
      new Rect(
          pos.x - 20,                   // posición x de la barra
          Screen.height - pos.y + 60,   // posición y de la barra
          40,                           // anchura de la barra    
          24                            // altura de la barra  
      ), health + "/" + maxHealth              // texto de la barra
  );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRage);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rageAtack);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, teleportZoneRage);
    }

}
