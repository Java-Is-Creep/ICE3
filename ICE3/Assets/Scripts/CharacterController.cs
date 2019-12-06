using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterController : MonoBehaviourPunCallbacks
{

    private float velocity = 5f;

    private int lastMovement = 0;
    public bool moving = false;

    // situacion en el cubo
    public int cara;
    public int indexX;
    public int indexY;

    public Vector3 target;


    float increment = 5f;

    //  scripts externos
    public Cube cubo;
    public GameObject bolaDeNieve;
    moverCamaraFija camaraScript;

    bool hecho = false;
    bool hayCambioCara;

    //Disparo
    public bool isFiring;
    public float timeBetweenShots;
    public float timeWaitingShots = 0;
    public int ammunition = 0;

    //Puntuacion
    private int puntos;
    public int MAXPUNTUACION;

    // Puntos
    private int puntosBolas;
    private int MaxPuntuacionBolas;



    bool ab = false;
    bool wb = false;
    bool sb = false;
    bool db = false;

    //Modelos
    GameObject model;
    [SerializeField]
    GameObject Bazoka;

    //Para colision entre personajes
    public int timeoutCollision;
    public int maxTimeoutCollision;
    //Para colision con banderas
    public int timeoutCollisionBanderas;
    public int maxTimeoutCollisionBanderas;
    // Para colision con bolas de nieve
    public int timeoutCollisionProyectil;
    public int maxTimeoutCollisionProyectil;

    //Para colision con bazoka
    public int timeoutCollisionBazoka;
    public int maxTimeoutCollisionBazoka;

    //Para colision con piedras
    public int timeoutCollisionRock;
    public int maxTimeoutCollisionRock;

    //Para colision con colliders de los borders
    public int maxTimeoutCollisionBorders;
    public int timeoutCollisionBorders;

    
    //spawns
    int indiceJugador;
    public CharacterController[] jugadores;
    ControladorNivel controladorNivel;
    //Para interfaz
    public Text textoBalas;
    public Text textoPuntos;

    //Para el audio durante el juego
    public gameSoundsController soundController;
    public bool sonidoEmpezado;

    // puntuaciones
    Puntuaciones punt;

    // tutorial
    bool tutorial = false;
    Vector3 CasillaTutorial = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
        punt = FindObjectOfType<Puntuaciones>();
        soundController = GameObject.Find("AudioController").GetComponent<gameSoundsController>();
        textoBalas = GameObject.Find("Balas").GetComponent<Text>();
        textoPuntos = GameObject.Find("PuntuacionTexto").GetComponent<Text>();
        Debug.Log("Cuantas veces he hecho el start" + " " + photonView.ViewID);
        model = this.transform.GetChild(0).gameObject;
        camaraScript = FindObjectOfType<moverCamaraFija>();
        maxTimeoutCollision = 3;
        maxTimeoutCollisionBanderas = 3;
        maxTimeoutCollisionProyectil = 3;
        maxTimeoutCollisionBazoka = 3;
        maxTimeoutCollisionRock = 3;
        maxTimeoutCollisionBorders = 3;
        timeoutCollision = 0;
        timeoutCollisionBanderas = 0;
        timeoutCollisionProyectil = 0;
        timeoutCollisionBazoka = 0;
        timeoutCollisionRock = 0;
        timeoutCollisionBorders = 0;
        puntos = 0;
        MAXPUNTUACION = 6;
        puntosBolas = 0;
        MaxPuntuacionBolas = 5;
        cara = -1;
        Bazoka.SetActive(false);


        //incializacion spawn
        controladorNivel = FindObjectOfType<ControladorNivel>();

        
        sonidoEmpezado = false;
        if (tutorial)
        {
            ColocarmeTutorial(CasillaTutorial);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (timeoutCollision > 0)
        {
            timeoutCollision--;
        }
        if (timeoutCollisionBanderas > 0)
        {
            timeoutCollisionBanderas--;
        }

        if (timeoutCollisionProyectil > 0)
        {
            timeoutCollisionProyectil--;
        }

        if (timeoutCollisionBazoka > 0)
        {
            timeoutCollisionBazoka--;
        }


        if (timeoutCollisionRock > 0)
        {
            timeoutCollisionRock--;
        }

        if (timeoutCollisionBorders > 0)
        {
            timeoutCollisionBorders--;
        }

        if (!photonView.IsMine)
        {
            return;
        }
        //actualizacion de variables
        timeWaitingShots += Time.deltaTime;
        isFiring = false;

        if (photonView.IsMine)
        {
            textoBalas.text = ammunition+"";
            textoPuntos.text = puntosBolas + "";
        }

        if (!hecho)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (controladorNivel != null)
                {
                    jugadores = FindObjectsOfType<CharacterController>();
                    if (jugadores.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
                    {
                        return;
                    }
                    Debug.Log("Cantidad de jugadores: " + jugadores.Length + " " + photonView.ViewID);
                    // llamamos a inicializar a ese jugador, ese jugador manda una rpc a todos y el que sea lo hará
                    jugadores[0].inicializateTu(photonView.ViewID,obtenerVector(controladorNivel.getCasillaVacia()));
                    hecho = true;
                    /*
                    object[] parametros = new object[2];
                    parametros[0] = jugadores[0].photonView.ViewID;
                    Debug.Log("ID a mandar: " + (int)parametros[0]);
                    parametros[1] = obtenerVector(controladorNivel.getCasillaVacia());
                    
                    this.photonView.RPC("Colocarme", RpcTarget.All, parametros);
                    */
                }
                else
                {
                    return;
                }

            }

          
        }

        if (!hecho)
        {
            return;
        }

        float incrementAux = increment * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            ComprobarDisparo();
        }


        //1
        if (Input.GetKeyDown("a") || ab)
        {
            //Debug.Log("He pulsado la A en boton movil: " + ab);
            if (cara == 0 || cara == 2)
            {
               model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 5)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            
            if (lastMovement == 0)
            {
                lastMovement = 1;
                
            }
            ab = false;
        }

        //2
        else if (Input.GetKeyDown("s") || sb)
        {
            //Debug.Log("He pulsado la S en boton movil: " + sb);
            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 5)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 2;
                
            }
            sb = false;
        }

        //3
        else if (Input.GetKeyDown("d") || db)
        {
            //Debug.Log("He pulsado la D en boton movil: " + db);
            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 5)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
           if (lastMovement == 0)
            {
                lastMovement = 3;
                
           }
            db = false;
        }

        //4
        else if (Input.GetKeyDown("w") || wb)
        {
            //Debug.Log("He pulsado la W en boton movil: " + wb);
            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 5)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 4;
                
            }
            wb = false;
        }


        switch (cara)
        {
            case 0:
                //Debug.Log("Indice cara top: " + indexX + ", " + indexY);
                MovimientoCaraTop(incrementAux);
                break;
            case 3:
                //Debug.Log("Indice cara right: " + indexX + ", " + indexY);
                MovimientoCaraRigth(incrementAux);
                break;
            case 2:
                //Debug.Log("Indice cara front: " + indexX + ", " + indexY);
                MovimientoCaraFront(incrementAux);
                break;
            case 4:
                //Debug.Log("Indice cara left: " + indexX + ", " + indexY);
                MovimientoCaraLeft(incrementAux);
                break;
            case 1:
                //Debug.Log("Indice cara back: " + indexX + ", " + indexY);
                MovimientoCaraBack(incrementAux);
                break;
            case 5:
                //Debug.Log("Indice cara bottom: " + indexX + ", " + indexY);
                MovimientoCaraBottom(incrementAux);
                break;

        }

    }

    public void ComprobarDisparo()
    {
        if (timeBetweenShots < timeWaitingShots)
        {
            if (ammunition > 0)
            {
                //Debug.Log("Disparando");
                this.photonView.RPC("Shot", RpcTarget.All, this.transform.position);
                timeWaitingShots = 0;
                soundController.playDisparo();
            }
            else
            {
                //Debug.Log("Sin municion");
                soundController.playSinBolas();
            }

        }
    }

    #region funcionalidd choques con objetos
    public void añadirBalas()
    {
        if (photonView.IsMine)
        {
            if (ammunition <= 0)
            {
                Bazoka.SetActive(true);
                this.photonView.RPC("SacarBazoka", RpcTarget.Others);
            }
            ammunition += 3;

        }

    }

    public void sumarPuntuacion()
    {
        puntos++;
        if (photonView.IsMine)
        {
            
            this.photonView.RPC("sumarPuntos", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        }
        Debug.Log("Puntos: " + puntos);
        if (puntos >= MAXPUNTUACION)
        {
            //Debug.Log("Puntos  de verdad: " + puntos);
            this.photonView.RPC("AcabarPartida", RpcTarget.All,PhotonNetwork.NickName);
        }

    }

    public void sumarPuntosBolas()
    {
        if(PlayerPrefs.GetInt("Modo") == 1)
        {
            puntosBolas++;
            if (photonView.IsMine)
            {
                /*
                Debug.Log("Cosas que yo mando");
                Debug.Log(PhotonNetwork.NickName);
                Debug.Log(PhotonNetwork.LocalPlayer.NickName);
                */
                this.photonView.RPC("sumarPuntos", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            }
            //Debug.Log("Puntos bolas: " + puntosBolas);
            if (puntosBolas >= MaxPuntuacionBolas)
            {
                this.photonView.RPC("AcabarPartida", RpcTarget.All, PhotonNetwork.NickName);
            }
        }

    }

    #endregion

    #region funciones auxiliares
    public void salirmePartida()
    {

       PhotonNetwork.LeaveRoom();
       PhotonNetwork.LeaveLobby();
        

    }

    public bool soyMaster()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public Vector3 obtenerVector(TileScript casilla)
    {
        return new Vector3(casilla.indexX, casilla.indexY, casilla.cubeId);
    }

    public void inicializateTu( int masterId, Vector3 casilla)
    {
        object[] parametros = new object[2];
        //Debug.Log("Holiwis");
        parametros[0] = masterId;
        //Debug.Log("ID a mandar: " + (int)parametros[0]);
        parametros[1] = casilla;
        this.photonView.RPC("Colocarme", RpcTarget.All, parametros);
    }

    // Manda RPC hasta que de con el master que llama al resto
    public void avisarAlSiguiente()
    {
        //Debug.Log("Manada siguiente orden");
        this.photonView.RPC("siguienteJugador", RpcTarget.MasterClient);
       
    }

    #endregion

    #region Colisiones
    private void OnTriggerEnter(Collider other)
    {
        if (soundController == null)
        {
            soundController = GameObject.Find("AudioController").GetComponent<gameSoundsController>();
        }
        if (other.tag == "KitBalas")
        {
            if(timeoutCollisionBazoka <= 0)
            {
                //Debug.Log("Balas Cogidas");
                soundController.playAccion();
                añadirBalas();
                timeoutCollisionBazoka = maxTimeoutCollisionBazoka;
                //other.gameObject.GetComponent<KitBalas>().crash();
            }
        }

        if (photonView.IsMine)
        {
            if (other.tag == "Bandera")
            {
                if (timeoutCollisionBanderas <= 0)
                {
                    //Debug.Log("Bandera Cogida");
                    sumarPuntuacion();
                    timeoutCollisionBanderas = maxTimeoutCollisionBanderas;
                }


            }

            if (other.tag == "Proyectil")
            {
                if (timeoutCollisionProyectil <= 0)
                {
                    if (other.GetComponent<Proyectil>().dueño != this.gameObject)
                    {
                        soundController.playRecibirBolazoOof();
                        timeoutCollisionProyectil = maxTimeoutCollisionProyectil;
                    }
                }

            }

            if(other.tag == "Rock")
            {
                if(timeoutCollisionRock <= 0)
                {
                    //Debug.Log("Yendo hacia arriba");
                    Vector3 aux = comprobarCasillaMasCercana();
                    //Debug.Log("antes: " + indexX + ", " + indexY + this.transform.position);
                    indexX = (int)aux.x;
                    indexY = (int)aux.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    //Caras top y bottom
                    if (cara == 0 || cara == 5)
                    {
                        this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    }
                    //Caras right y left
                    else if (cara == 3 || cara == 4)
                    {
                        this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    }
                    //Caras front y back
                    else if (cara == 1 || cara == 2)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    }
                    //Debug.Log("ahora: " + indexX + ", " + indexY + this.transform.position);
                    lastMovement = 0;
                    moving = false;

                    Debug.Log("Choque por roca");

                    timeoutCollisionRock = maxTimeoutCollisionRock;
                }
            }

            if (other.tag == "CharacterCollider")
            {
                if (timeoutCollision <= 0)
                {
                    soundController.playChoque();
                    //Debug.Log("Colision con personaje");
                    timeoutCollision = maxTimeoutCollision;
                    hayCambioCara = false;
                    //Si estamos en w, ponemos 2
                    if (lastMovement == 4)
                    {
                        //Debug.Log("Yendo hacia arriba");
                        Vector3 aux = comprobarCasillaMasCercana();
                        //Debug.Log("antes: " + indexX + ", " + indexY + this.transform.position);
                        indexX = (int)aux.x;
                        indexY = (int)aux.y;
                        Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                        //Caras top y bottom
                        if (cara == 0 || cara == 5)
                        {
                            this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                        }
                        //Caras right y left
                        else if (cara == 3 || cara == 4)
                        {
                            this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                        }
                        //Caras front y back
                        else if (cara == 1 || cara == 2)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                        }
                        //Debug.Log("ahora: " + indexX + ", " + indexY + this.transform.position);
                        lastMovement = 2;
                        moving = false;
                    }
                    else if (lastMovement == 2)
                    {
                        Debug.Log("Yendo hacia abajo");
                        Vector3 aux = comprobarCasillaMasCercana();
                        Debug.Log("antes: " + indexX + ", " + indexY + this.transform.position);
                        indexX = (int)aux.x;
                        indexY = (int)aux.y;
                        Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                        //Caras top y bottom
                        if (cara == 0 || cara == 5)
                        {
                            this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                        }
                        //Caras right y left
                        else if (cara == 3 || cara == 4)
                        {
                            this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                        }
                        //Caras front y back
                        else if (cara == 1 || cara == 2)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                        }
                        Debug.Log("ahora: " + indexX + ", " + indexY + this.transform.position);
                        lastMovement = 4;
                        moving = false;
                    }
                    else if (lastMovement == 3)
                    {
                        Debug.Log("Yendo hacia derecha");
                        Vector3 aux = comprobarCasillaMasCercana();
                        Debug.Log("antes: " + indexX + ", " + indexY + this.transform.position);
                        indexX = (int)aux.x;
                        indexY = (int)aux.y;
                        Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                        //Caras top y bottom
                        if (cara == 0 || cara == 5)
                        {
                            this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                        }
                        //Caras right y left
                        else if (cara == 3 || cara == 4)
                        {
                            this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                        }
                        //Caras front y back
                        else if (cara == 1 || cara == 2)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                        }
                        Debug.Log("ahora: " + indexX + ", " + indexY + this.transform.position);
                        lastMovement = 1;
                        moving = false;
                    }
                    else if (lastMovement == 1)
                    {
                        //Debug.Log("Yendo hacia izquierda");
                        Vector3 aux = comprobarCasillaMasCercana();
                        //Debug.Log("antes: " + indexX + ", " + indexY + this.transform.position);
                        indexX = (int)aux.x;
                        indexY = (int)aux.y;
                        Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                        //Caras top y bottom
                        if (cara == 0 || cara == 5)
                        {
                            this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                        }
                        //Caras right y left
                        else if (cara == 3 || cara == 4)
                        {
                            this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                        }
                        //Caras front y back
                        else if (cara == 1 || cara == 2)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                        }
                        Debug.Log("ahora: " + indexX + ", " + indexY + this.transform.position);
                        lastMovement = 3;
                        moving = false;
                    }
                }
            }

            if (other.tag == "ColisionBorde")
            {
                if (timeoutCollisionBorders <= 0)
                {
                    Debug.Log("Colision con borde");
                    //Cara top
                    if (cara == 0)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, -0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 4;
                            moving = false;
                            lastMovement = 2;
                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            indexY = 7;
                            hayCambioCara = false;
                        }
                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            cara = 2;
                            moving = false;
                            //lastMovement = 0;
                            indexX = 0;
                            hayCambioCara = false;
                        }
                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, 0.5f), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = 0;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 2;
                            hayCambioCara = false;
                        }
                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            cara = 1;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 7;
                            hayCambioCara = false;
                        }
                    }

                    //back
                    else if (cara == 1)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 0.5f), -0.5f, 0), Space.World);
                            cara = 5;
                            lastMovement = 3;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            hayCambioCara = false;
                        }
                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = /*((int)(cubo.width - 1)) -*/ indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1.5f), -0.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 0;
                            hayCambioCara = false;
                        }
                    }

                    //Front
                    else if (cara == 2)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 7;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 0.5f), -0.5f, 0), Space.World);
                            cara = 5;
                            lastMovement = 1;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 7;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }

                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            hayCambioCara = false;
                        }
                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(this.transform.position.x, tile.AbsolutePos.y, tile.AbsolutePos.z);
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1.5f), -0.5f, 0), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexX = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 4;
                            hayCambioCara = false;
                        }
                    }

                    //Right
                    else if (cara == 3)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                            cara = 2;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }

                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, +(cubo.width - 0.5f)), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            cara = 5;
                            moving = false;
                            lastMovement = 4;
                            indexY = 0;
                            hayCambioCara = false;
                        }

                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, (cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 7;


                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }

                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }

                    //Left
                    else if (cara == 4)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = /*((int)(cubo.width - 1)) -*/ indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 0.5f)), Space.World);
                            cara = 5;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);

                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 2;
                            hayCambioCara = false;
                        }
                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 2;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux2 = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux2.x, (int)aux2.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 1.5f)), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            lastMovement = 3;
                            indexY = 0;
                            hayCambioCara = false;
                        }
                    }

                    //Bottom
                    else if (cara == 5)
                    {
                        //a
                        if (lastMovement == 1)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(+0.5f, -(cubo.width - 0.5f), 0), Space.World);
                            cara = 1;
                            lastMovement = 4;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            hayCambioCara = false;
                        }
                        //s
                        else if (lastMovement == 2)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), -0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 3;
                            moving = false;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            lastMovement = 4;
                            indexY = 7;
                            hayCambioCara = false;
                        }
                        //d
                        else if (lastMovement == 3)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-0.5f, -(cubo.width - 0.5f), 0), Space.World);
                            cara = 2;
                            lastMovement = 4;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 7;
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);

                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            hayCambioCara = false;
                        }
                        //w
                        else if (lastMovement == 4)
                        {
                            Vector3 aux = comprobarCasillaMasCercana();
                            TileScript tile = cubo.faces[cara].tiles[(int)aux.x, (int)aux.y].GetComponent<TileScript>();
                            this.transform.position = new Vector3(tile.AbsolutePos.x, this.transform.position.y, tile.AbsolutePos.z);
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), 0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 4;
                            moving = false;
                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            lastMovement = 4;
                            indexY = 0;
                            hayCambioCara = false;
                        }
                    }

                    timeoutCollisionBorders = maxTimeoutCollisionBorders;
                }

            }
        }

    }

    private Vector3 comprobarCasillaMasCercana()
    {
        int indexXcerca = -1;
        int indexYcerca = -1;
        float distanciaMasCercana = 100000f;
        Vector3 posTile;
        for (int i = 0; i <= cubo.width - 1; i++)
        {
            for (int j = 0; j <= cubo.heigth - 1; j++)
            {
                posTile = cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().AbsolutePos;
                float aux = Vector3.Distance(this.transform.position, posTile);
                if (aux < distanciaMasCercana)
                {
                    distanciaMasCercana = aux;
                    indexXcerca = i;
                    indexYcerca = j;
                }
            }
        }
        return new Vector3(indexXcerca, indexYcerca, distanciaMasCercana);
    }
    #endregion

    #region RPCs


    [PunRPC]
    void sumarPuntos(string nick)
    {
        Debug.Log("El nick que me han pasado es: " + nick);
        Debug.Log("me mandan la RPC de sumar puntos");
        if(punt == null)
        {
            punt = FindObjectOfType<Puntuaciones>();
        }
        punt.anadirPunto(nick);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="posicion"></param>
    [PunRPC]
    void Shot(Vector3 posicion)
    {
        /* if(targetID == photonView.GetInstanceID())
         {
             GameObject aux = Instantiate(bolaDeNieve, this.transform.position + (Vector3.forward * 0.2f), Quaternion.identity);
             aux.GetComponent<Proyectil>().initDireccion(this.gameObject.transform.TransformDirection(Vector3.forward), this.gameObject);
         }*/

        soundController.playDisparo();

        Transform childTransform = this.gameObject.transform.GetChild(0);
        GameObject aux = Instantiate(bolaDeNieve, posicion + (childTransform.TransformDirection(Vector3.back * 0.2f)), Quaternion.identity);

        Proyectil proyectil = aux.GetComponent<Proyectil>();
        if (cubo == null)
        {
            cubo = FindObjectOfType<Cube>();
        }
        proyectil.initDireccion(childTransform.TransformDirection(Vector3.back), this.gameObject, cubo.heigth);

        //Moverte cuando disparas
        hayCambioCara = false;
        Vector3 aux2;

        //Cara TOP
        if (cara == 0)
        {
            //Estas mirando hacia arriba
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 2;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la izquierda
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia dcha");
                    lastMovement = 3;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la derecha
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izqda");
                    lastMovement = 1;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia abajo
            else
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 4;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }
        }

        //Cara back
        else if (cara == 1)
        {
            Debug.Log("Cara back");
            //Estas mirando hacia abajo
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 4;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la derecha
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia dcha");
                    lastMovement = 1;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la izqda
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izqda");
                    lastMovement = 3;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia arriba
            else
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 2;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }
        }

        //Cara front
        else if (cara == 2)
        {
            Debug.Log("Cara front");
            //Estas mirando hacia abajo
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 2;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la derecha
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia dcha");
                    lastMovement = 3;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la izqda
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izqda");
                    lastMovement = 1;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia arriba
            else
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 4;
                    this.transform.position = new Vector3(this.transform.position.x, posTile.y, posTile.z);
                    moving = false;
                }
            }
        }

        //Cara right
        else if (cara == 3)
        {
            Debug.Log("Cara derecha");
            //Estas mirando hacia abajo
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia derecha");
                    lastMovement = 1;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia la arriba
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 2;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia abajo
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 4;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia izqda
            else
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izqda");
                    lastMovement = 3;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }
        }

        //Cara left
        else if (cara == 4)
        {
            Debug.Log("Cara left");
            //Estas mirando hacia izquierda
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izquierda");
                    lastMovement = 3;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia la abajo
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 4;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia arriba
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 2;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }

            //Mirando hacia dcha
            else
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia dcha");
                    lastMovement = 1;
                    this.transform.position = new Vector3(posTile.x, posTile.y, this.transform.position.z);
                    moving = false;
                }
            }
        }

        //Cara bottom
        else if (cara == 5)
        {
            //Estas mirando hacia izquierda
            if (Mathf.Abs(model.transform.localEulerAngles.y - 90) <= 1)
            {
                if (lastMovement != 3)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia izquierda");
                    lastMovement = 3;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la abajo
            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
            {
                if (lastMovement != 4)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia abajo");
                    lastMovement = 4;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia la arriba
            else if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
            {
                if (lastMovement != 2)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia arriba");
                    lastMovement = 2;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }

            //Mirando hacia dcha
            else
            {
                if (lastMovement != 1)
                {
                    aux2 = comprobarCasillaMasCercana();
                    indexX = (int)aux2.x;
                    indexY = (int)aux2.y;
                    Vector3 posTile = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
                    Debug.Log("Estoy mirando hacia dcha");
                    lastMovement = 1;
                    this.transform.position = new Vector3(posTile.x, this.transform.position.y, posTile.z);
                    moving = false;
                }
            }
        }
        if (photonView.IsMine)
        {
            ammunition--;
            if (ammunition <= 0)
            {
                Bazoka.SetActive(false);
                this.photonView.RPC("QuitarBazoka", RpcTarget.Others);
                
            }
        }


    }

    [PunRPC]
    void AcabarPartida(string ganador)
    {
        if (photonView.IsMine)
        {
            PlayerPrefs.SetString("ganador", ganador);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameOver");
        }
        //Debug.Log("Veces que se llama");
        //salirmePartida();
    }

    public void activarTutorial(Vector3 datosCasilla)
    {
        tutorial = true;
        this.CasillaTutorial = datosCasilla;
    }

    public void ColocarmeTutorial(Vector3 datosCasilla)
    {
        Debug.Log("Tutorial");
        camaraScript = FindObjectOfType<moverCamaraFija>();
        cubo = FindObjectOfType<Cube>();

        TileScript ts = cubo.faces[(int)datosCasilla.z].tiles[(int)datosCasilla.x, (int)datosCasilla.y].GetComponent<TileScript>();

        indexX = ts.indexX;
        indexY = ts.indexY;
        cara = ts.cubeId;


        this.transform.position = ts.AbsolutePos;
        switch (cara)
        {
            case (0):

                break;
            case (1):
                this.transform.Rotate(new Vector3(0, 0, 90));
                camaraScript.back();
                break;
            case (2):
                this.transform.Rotate(new Vector3(0, 0, -90));
                camaraScript.front();
                break;
            case (3):
                this.transform.Rotate(new Vector3(90, 0, 0));
                camaraScript.right();
                break;
            case (4):
                this.transform.Rotate(new Vector3(-90, 0, 0));
                camaraScript.left();
                break;
            case (5):
                //Debug.Log("Cambiando de cara");
                this.transform.Rotate(new Vector3(-180, 0, 0));
                camaraScript.button();
                break;
        }
        this.transform.position += this.transform.TransformDirection(Vector3.up);
        hecho = true;
    }

    [PunRPC]
    void Colocarme(object[] parametros)
    {
        // este personaje se actualiza solo en su cliente, ahora hay que avisar al cliente de que avise a otros.
        if (photonView.IsMine)
        {
            int idMaster = (int)parametros[0];
            Vector3 datosCasilla = (Vector3)parametros[1];
            Debug.Log("Photon view a colocar: " + this.photonView.ViewID );
            Debug.Log("Posiciones al que mandarlo" + datosCasilla + " " + this.photonView.ViewID);
            Debug.Log(photonView.ViewID + " es mi photon id" + " " + this.photonView.ViewID);
            int aux = this.photonView.ViewID;

            camaraScript = FindObjectOfType<moverCamaraFija>();
            cubo = FindObjectOfType<Cube>();

            TileScript ts = cubo.faces[(int)datosCasilla.z].tiles[(int)datosCasilla.x, (int)datosCasilla.y].GetComponent<TileScript>();

            indexX = ts.indexX;
            indexY = ts.indexY;
            cara = ts.cubeId;


            this.transform.position = ts.AbsolutePos;
            switch (cara)
            {
                case (0):

                    break;
                case (1):
                    this.transform.Rotate(new Vector3(0, 0, 90));
                    camaraScript.back();
                    break;
                case (2):
                    this.transform.Rotate(new Vector3(0, 0, -90));
                    camaraScript.front();
                    break;
                case (3):
                    this.transform.Rotate(new Vector3(90, 0, 0));
                    camaraScript.right();
                    break;
                case (4):
                    this.transform.Rotate(new Vector3(-90, 0, 0));
                    camaraScript.left();
                    break;
                case (5):
                    //Debug.Log("Cambiando de cara");
                    this.transform.Rotate(new Vector3(-180, 0, 0));
                    camaraScript.button();
                    break;
            }
            this.transform.position += this.transform.TransformDirection(Vector3.up);
            hecho = true;
            Debug.Log("Mando inicializar a otro");
            jugadores = FindObjectsOfType<CharacterController>();
            CharacterController aux2 = null;

            foreach(CharacterController controlador in jugadores)
            {
                if(controlador.photonView.ViewID == idMaster)
                {
                    /*
                    Dictionary<int,Player> aux3 =   PhotonNetwork.CurrentRoom.Players;
                    foreach(Player play in aux3.Values)
                    {
                        Debug.Log(play.NickName);
                        play.SetCustomProperties()
                    }
                    */
                    aux2 = controlador;
                    Debug.Log("Encontrado master, su id es " + idMaster);
                }
            }

            aux2.avisarAlSiguiente();



        }

    }

    [PunRPC]
    void siguienteJugador()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            indiceJugador++;
            Debug.Log(indiceJugador + "indice jugador");
            if (indiceJugador < jugadores.Length)
            {
                jugadores[indiceJugador].inicializateTu(photonView.ViewID,obtenerVector(controladorNivel.getCasillaVacia()));
            }

        } else
        {
            Debug.Log("No soy el master");
        }

    }

    [PunRPC]
    void SacarBazoka()
    {
        Bazoka.SetActive(true);
    }

    [PunRPC]
    void QuitarBazoka()
    {
        Bazoka.SetActive(false);
    }



    #endregion

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main Menu");
    }

    #region Movimientos
    public void MovimientoCaraTop(float incrementAux)
    {
        //Arriba
        if (lastMovement == 4) // arriba
        {
            //Debug.Log("Arriba");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);

                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            cara = 1;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 7;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                            /*
                            for (int i = 0; i < cubo.width; i++)
                            {
                                for (int j = 0; j < cubo.width; j++)
                                {
                                    Debug.Log(i + " " + j + " " + cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().myObjectType);
                                }
                            }*/
                        }
                    }
                }
                else
                {

                    if (indexX > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);


                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3(-0.5f, cubo.width - 1.5f, 0), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                        cara = 1;
                        moving = false;
                        //del 0,1 al 1,7
                        lastMovement = 2;
                        //indexX = indexY;
                        indexX = 7;
                        hayCambioCara = false;
                    }


                }

            }



        }

        //Izquierda
        else if (lastMovement == 1) // izqda
        {
            //Debug.Log("Izqda");

            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);

                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        //Debug.Log("He llegado a la casilla");
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        //lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, -0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 4;
                            moving = false;
                            lastMovement = 2;
                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            indexY = 7;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                            /*
                            for (int i = 0; i < cubo.width; i++)
                            {
                                for (int j = 0; j < cubo.width; j++)
                                {
                                    Debug.Log(cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().myObjectType);
                                }
                            }*/
                        }

                    }
                }
                else
                {
                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    //Debug.Log("Iteraciones: " + iteracion);
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log(tile.AbsolutePos);
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        //Debug.Log(tile.AbsolutePos);
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);
                        //Debug.Log("Target es:" + target);
                        
                    }
                    else
                    {
                        // Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, -0.5f), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 4;
                        moving = false;
                        this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        lastMovement = 2;
                        indexY = 7;
                        hayCambioCara = false;
                    }


                }

            }
        }

        //Abajo
        else if (lastMovement == 2)
        {
            //Debug.Log("abajo");
            if (indexX < cubo.width)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);

                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            cara = 2;
                            moving = false;
                            //lastMovement = 0;
                            indexX = 0;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {

                    if (indexX < cubo.width - 1)
                    {
                        int iteracion = 0;
                        TileScript tile;

                        do
                        {
                            // Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);
                        

                    }
                    else
                    {
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                        this.gameObject.transform.Translate(new Vector3(0.5f, cubo.width - 1.5f, 0), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 2;
                        moving = false;
                        //lastMovement = 0;
                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                        indexX = 0;
                        hayCambioCara = false;
                        //Debug.LogWarning("Cambio de cara");

                    }
                }

            }

        }

        //Dcha
        else if (lastMovement == 3)
        {

            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, 0.5f), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = 0;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 2;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);
                        
                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, 0.5f), Space.World);
                        cara = 3;
                        //indexX = 1;
                        indexY = 0;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 2;
                        hayCambioCara = false;

                    }

                }

            }
        }
    }

    public void MovimientoCaraRigth(float incrementAux)
    {
        // izq
        if (lastMovement == 1)
        {
            //Debug.Log("Izq");
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                            cara = 2;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                        cara = 2;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;
                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);

                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }


                }

            }

        }

        // derecha
        else if (lastMovement == 3)
        {
            //Debug.Log("Dcha");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        // Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, (cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 7;


                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    // Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, (cubo.width - 1f)), Space.World);
                        cara = 1;
                        //indexX = 1;
                        indexX = ((int)(cubo.width - 1)) - indexY;
                        indexY = 7;


                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                    }
                    //Debug.Log("Hola");

                }

            }

        }

        //Abajo
        else if (lastMovement == 2)
        {
            //Debug.Log("Abajo");
            //Debug.Log("Estoy en " + indexY);
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, +(cubo.width - 0.5f)), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            cara = 5;
                            moving = false;
                            lastMovement = 4;
                            indexY = 0;
                            hayCambioCara = false;
                            sonidoEmpezado = true;

                        }
                    }
                }
                else
                {

                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.button();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, +(cubo.width - 0.5f)), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        cara = 5;
                        moving = false;
                        lastMovement = 4;
                        indexY = 0;
                        hayCambioCara = false;
                    }
                }

            }

        }

        //Arriba
        else if (lastMovement == 4)
        {
            //Debug.Log("Arriba");
            //Debug.Log("Estoy en " + indexY);
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {

                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                        cara = 0;
                        indexY = 7;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }
                }

            }

        }

    }

    public void MovimientoCaraFront(float incrementAux)
    {

        //Dcha
        if (lastMovement == 3)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY+1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                        cara = 3;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                        //Debug.LogWarning("Cambio de cara");
                    }

                }

            }
        }

        //Izquierda
        if (lastMovement == 1)
        {
            //Debug.Log("SALE");
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 7;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);

                    }
                    else
                    {
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3((cubo.width - 1f), 0, 0), Space.World);
                        cara = 4;
                        //indexX = 1;
                        indexY = ((int)(cubo.width - 1)) - indexX;
                        indexX = 7;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }

                }

            }
        }

        //Abajo
        if (lastMovement == 2)
        {

            if (indexX < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 0.5f), -0.5f, 0), Space.World);
                            cara = 5;
                            lastMovement = 1;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 7;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }

                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            // Debug.Log("Leyendo casilla: " + (indexX + 1) + ", " + (indexY ));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                // Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.button();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                        this.gameObject.transform.Translate(new Vector3((cubo.width - 0.5f), -0.5f, 0), Space.World);
                        cara = 5;
                        lastMovement = 1;
                        indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                        indexX = 7;
                        this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

                        if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                        }

                        //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        hayCambioCara = false;
                    }

                }

            }
        }

        //Arriba
        if (lastMovement == 4)
        {

            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1.5f), -0.5f, 0), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexX = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 4;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }

                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            // Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3((cubo.width - 1.5f), -0.5f, 0), Space.World);
                        cara = 0;
                        indexX = 7;
                        moving = false;
                        lastMovement = 4;
                        hayCambioCara = false;
                    }

                }

            }
        }
    }

    public void MovimientoCaraLeft(float incrementAux)
    {
        //izqda
        if (lastMovement == 1)
        {
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        // Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = /*((int)(cubo.width - 1)) -*/ indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    // Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                        cara = 1;
                        //indexX = 1;
                        indexX = /*((int)(cubo.width - 1)) -*/ indexY;
                        indexY = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }
                    //Debug.Log("Hola");

                }

            }
        }

        //Abajo
        else if (lastMovement == 2)
        {
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {

                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 0.5f)), Space.World);
                            cara = 5;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);

                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 2;
                            hayCambioCara = false;

                            sonidoEmpezado = true;
                            /*
                            for (int i = 0; i < cubo.width; i++)
                            {
                                for (int j = 0; j < cubo.width; j++)
                                {
                                    Debug.Log(i + " " + j + " " + cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().myObjectType);
                                }
                            }*/
                        }
                    }
                }
                else
                {

                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.button();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 0.5f)), Space.World);
                        cara = 5;
                        //indexX = 1;
                        indexY = 7;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;
                        this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 0, 0);

                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 2;
                        hayCambioCara = false;
                    }
                }

            }
        }

        //derecha
        else if (lastMovement == 3)
        {
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {

                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 2;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                        cara = 2;
                        //indexX = 1;
                        indexX = ((int)(cubo.width - 1)) - indexY;
                        indexY = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                        /*
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                        cara = 2;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                    
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                        hayCambioCara = false;*/
                    }


                }

            }
        }

        //arriba
        else if (lastMovement == 4)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 1.5f)), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            lastMovement = 3;
                            indexY = 0;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {

                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;

                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 1.5f)), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 0;
                        moving = false;
                        lastMovement = 3;
                        indexY = 0;
                        hayCambioCara = false;
                    }
                }

            }
        }
    }

    public void MovimientoCaraBack(float incrementAux)
    {
        //izquierda
        if (lastMovement == 1)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                // Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                        cara = 3;
                        //indexX = 1;
                        indexY = ((int)(cubo.width - 1)) - indexX;
                        indexX = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 90, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }

                }

            }
        }

        //abajo
        else if (lastMovement == 2)
        {

            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 0.5f), -0.5f, 0), Space.World);
                            cara = 5;
                            lastMovement = 3;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.button();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 0.5f), -0.5f, 0), Space.World);
                        cara = 5;
                        lastMovement = 3;
                        indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                        indexX = 0;
                        this.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
                        if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                        }
                        //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        hayCambioCara = false;
                    }

                }

            }
        }

        //derecha
        else if (lastMovement == 3)
        {
            //Debug.Log("SALE");
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = /*((int)(cubo.width - 1)) -*/ indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);

                    }
                    else
                    {
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                        cara = 4;
                        //indexX = 1;
                        indexY = /*((int)(cubo.width - 1)) -*/ indexX;
                        indexX = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y - 90, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                    }

                }

            }
        }

        //arriba
        else if (lastMovement == 4)
        {
            if (indexX < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1.5f), -0.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 0;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                            /*
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;*/
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX + 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1.5f), -0.5f, 0), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 0;
                        moving = false;
                        //del 0,1 al 1,7
                        lastMovement = 2;
                        //indexX = indexY;
                        indexX = 0;
                        hayCambioCara = false;
                    }

                }

            }
        }
    }

    public void MovimientoCaraBottom(float incrementAux)
    {
        //Abajo
        if (lastMovement == 2)
        {
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), -0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 3;
                            moving = false;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            lastMovement = 4;
                            indexY = 7;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {

                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), -0.5f), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 3;
                        moving = false;
                        this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        lastMovement = 4;
                        indexY = 7;
                        hayCambioCara = false;
                    }
                }

            }
        }

        //arriba
        else if (lastMovement == 4)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), 0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 4;
                            moving = false;
                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            lastMovement = 4;
                            indexY = 0;
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                }
                else
                {

                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);

                    }
                    else
                    {
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, -(cubo.width - 0.5f), 0.5f), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 4;
                        moving = false;
                        this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        //model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        lastMovement = 4;
                        indexY = 0;
                        hayCambioCara = false;
                    }
                }

            }
        }

        //derecha
        else if (lastMovement == 3)
        {
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-0.5f, -(cubo.width - 0.5f), 0), Space.World);
                            cara = 2;
                            lastMovement = 4;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 7;
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);

                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3(-0.5f, -(cubo.width - 0.5f), 0), Space.World);
                        cara = 2;
                        lastMovement = 4;
                        indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                        indexX = 7;
                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                        //model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        hayCambioCara = false;
                    }


                }

            }
        }

        //izqda
        if (lastMovement == 1)
        {
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (!sonidoEmpezado)
                    {
                        soundController.playDeslizar();
                        sonidoEmpezado = true;
                    }
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        sonidoEmpezado = false;
                        this.transform.position = target;
                        target = this.transform.position;
                        // Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(+0.5f, -(cubo.width - 0.5f), 0), Space.World);
                            cara = 1;
                            lastMovement = 4;
                            indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                            indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                            {
                                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            }
                            else
                            {
                                model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                            }
                            //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            hayCambioCara = false;
                            sonidoEmpezado = true;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType != TileScript.tileObject.ROCK)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    // Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                        sonidoEmpezado = false;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                        this.gameObject.transform.Translate(new Vector3(+0.5f, -(cubo.width - 0.5f), 0), Space.World);
                        cara = 1;
                        lastMovement = 4;
                        indexY = (Mathf.RoundToInt(cubo.width) - 1) - indexY;
                        indexX = 0;
                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                        if (Mathf.Abs(model.transform.localEulerAngles.y - 180) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Mathf.Abs(model.transform.localEulerAngles.y) <= 1)
                        {
                            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            model.transform.localRotation = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0);
                        }
                        //model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        hayCambioCara = false;
                    }
                    //Debug.Log("Hola");

                }

            }
        }
    }

    #endregion

    #region botones para movil
    public void w()
    {
        
            wb = true;
        
    }

    public void a()
    {
        
            ab = true;
        
    }

    public void s()
    {
        
            sb = true;
        
    }

    public void d()
    {
        
            db = true;
        
    }
    #endregion

}
