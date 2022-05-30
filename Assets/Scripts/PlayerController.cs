using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Este es controlador del jugador, aquí se encuentra las funciones de movimiento, colisiones con otros objetos,
/// registro de aciertos y fallidos, como su precisión. Así también, en las colisiones detecta cuando entra contacto con el bloque correcto e incorrecto
/// y el pase a otros niveles del minijuego de sucesiones; y sumas y restas
/// </summary>
public class PlayerController : MonoBehaviour
{
    
    //Variables del movimiento del personaje
    public float jumpForce = 6f;
    private float distanceRay = 1f;
    public float runningSpeed = 2f;
    private float contadorFallido;
    private float contadorAciertos;
    private float contadorTotal;
    private float precision;
    Vector3 startPosition;
    private Rigidbody2D rigidBody;
    public float horizontalMove = 0;
    public float runSpeed = 0;
    public Joystick joystick;
    float a;
    private float posJoystick;
    private int state;
    //Ahora para agregarle animaci�n al personaje dependiendo de su estado
    Animator animator;
    //private  TimeController timeController;
    //Para el sonido
    public AudioSource clip;
    // Para asignar al boton "saltar"
    //[SerializeField] public Button jump;
    //Necesito referenciar las variables booleanas que he creado en Unity (IsOnTheGround)

    private const string STATE_ON_THE_GROUND = "IsOnTheGround";
    private const string STATE_IS_WALKING = "IsWalking";
    
    public LayerMask groundMask;

    private bool isSucesion;
    private bool isSumasRestas;
    [SerializeField] public bool comenzo;
    //private string comenzoPrefsName = "comenzó";
    [Header("RESULT VARIABLES")]
    public GameObject resultPanel;
    public Text scoreText;
    public Text timeText;
    
    [Header("JOYSTICK AND JUMP")]
    public GameObject actJoyStick;
    public GameObject actJump;

    
    void Awake()
    {
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        loadDataContadores();
        print("contadorFallido: " + contadorFallido);
        print("contadorAciertos: " + contadorAciertos);
    }

    // Start is called before the first frame update
    void Start()    
    {
      
        startPosition = this.transform.position;
       
    }

    /// <summary>
    /// Inicia el minujuego seleccionado
    /// </summary>
    public void StartGame()
    {   //Cuando empezamos a jugar el jugador no estará en el suelo
        animator.SetBool(STATE_ON_THE_GROUND, false);
        animator.SetBool(STATE_IS_WALKING, false); 
        this.transform.position = startPosition;
        //Esto se pone para que la velocidad sea igual a la de la inicial
        this.rigidBody.velocity = Vector2.zero;
    }
    

    
    /// <summary>
    /// Se obtiene la información de los contadores de aciertos y fallidos
    /// </summary>
    private void loadDataContadores()
    {
        contadorFallido = ContController.getContFailure();
        contadorAciertos = ContController.getContSuccess();

    }

    // Update is called once per frame
    void Update()
    {
       
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

    }

    
    /// <summary>
    /// En esta función, se detecta los movimientos hechos en el joystick y el personaje deberá moverse para el lado correcto
    /// </summary>
    private void FixedUpdate()
    {
        
        if (joystick.Horizontal<=0.01f)
        {
            a = joystick.Horizontal;
        }
        else if(joystick.Horizontal>0.01f)
        {
            a = joystick.Horizontal;
        }

        horizontalMove = a * runningSpeed;
        transform.position += new Vector3(horizontalMove, 0,0) * Time.deltaTime * runSpeed;
        if (joystick.Horizontal>=0.1)
        {
            
            state = 1; // derecha
            posJoystick = joystick.Horizontal;
        }
        else if (joystick.Horizontal<0)
        {
            
            state = 2; // izquierda
            posJoystick = joystick.Horizontal;
        }
        else 
        {
            state = 3; // parado
        }

        
        switch (state)
        {
           case 1:
               rigidBody.GetComponent<SpriteRenderer>().flipX = false;
               animator.SetBool(STATE_IS_WALKING, true);
               break;
           case 2:
                rigidBody.GetComponent<SpriteRenderer>().flipX = true;
                animator.SetBool(STATE_IS_WALKING, true);
               break;
           case 3:
                animator.SetBool(STATE_IS_WALKING, false);
                break;
        }
        
       
    }

    /// <summary>
    /// Es un método que si o si se va a utilizar, se encarga de controlar los saltos.
    /// Esta asignado al botón de la interfaz del juego
    /// </summary>
    public void Jump()
    {
        animator.SetBool(STATE_IS_WALKING, false);

        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
        }

    
    }
    
    /// <summary>
    /// Para esto debo crear un nuevo Layer llamado Ground para todas las plataformas
    /// Esto nos va a servir para que el salto no se haga consecutivo
    /// </summary>
    /// <returns></returns>
    bool IsTouchingTheGround()
    {   //this.position = desde la posici�n desde donde hasta donde quiero trazar el rayo (down) 
        if (Physics2D.Raycast(this.transform.position, Vector2.down, distanceRay,groundMask))
        {       
            //GameManager.sharedInstance.currentGameState = GameManager.GameState.inGame;
            
            return true;
            
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Se resetea los valores de aciertos y fallidos, al salir de la aplicación
    /// </summary>
    private void OnApplicationQuit()
    {
        
        ContController.setContFailure(0);
        ContController.setContSuccess(0);
    }

    /// <summary>
    /// Esta parte es importante, aquí se maneja cuando el jugador colisiona con el bloque correcto e incorrectos, de los minijuegos de sucesiones; y sumas y restas.
    /// Se maneja la transición de niveles y dificultades, contadores de aciertos y fallidos.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Incorrect"))
        {
            contadorFallido += 1;
            ContController.setContFailure(contadorFallido);
            print("contadorFailure: " + ContController.getContFailure());
            Puntaje.puntaje.RestarPuntos(10);
        }
        else if (collision.CompareTag("Correct"))
        {
            Puntaje.puntaje.SumarPuntos(100);
            contadorAciertos += 1;
            ContController.setContSuccess(contadorAciertos);
            print("contadorAciertos: " + ContController.getContSuccess());
            // FACIL NIVEL 1 sucesiones
            if (Application.loadedLevelName == "Sucesiones_Facil_1")
            {
                SceneManager.LoadScene("Sucesiones_Facil_2", LoadSceneMode.Single);
            } 
            else if (Application.loadedLevelName == "Sucesiones_Facil_2") SceneManager.LoadScene("Sucesiones_Facil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Facil_3") SceneManager.LoadScene("Sucesiones_Facil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Facil_4") SceneManager.LoadScene("Sucesiones_Facil_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Facil_5") //MEDIO NIVEL 1
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0,(int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
                
            }
            if (Application.loadedLevelName == "Sucesiones_Medio_1") SceneManager.LoadScene("Sucesiones_Medio_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Medio_2") SceneManager.LoadScene("Sucesiones_Medio_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Medio_3") SceneManager.LoadScene("Sucesiones_Medio_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Medio_4") SceneManager.LoadScene("Sucesiones_Medio_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Medio_5")  // DIFICIL NIVEL 1
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0, (int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
            }
            else if (Application.loadedLevelName == "Sucesiones_Dificil_1") SceneManager.LoadScene("Sucesiones_Dificil_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Dificil_2") SceneManager.LoadScene("Sucesiones_Dificil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Dificil_3") SceneManager.LoadScene("Sucesiones_Dificil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Dificil_4") SceneManager.LoadScene("Sucesiones_Dificil_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sucesiones_Dificil_5")
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0, (int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
            }
            // FACIL NIVEL 1 sumas y restas
            if (Application.loadedLevelName == "Sumas_y_restas_facil_1") SceneManager.LoadScene("Sumas_y_restas_facil_2", LoadSceneMode.Single); 
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_2") SceneManager.LoadScene("Sumas_y_restas_facil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_3") SceneManager.LoadScene("Sumas_y_restas_facil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_4") SceneManager.LoadScene("Sumas_y_restas_facil_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_5") //MEDIO NIVEL 1
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0, (int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
            }
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_1") SceneManager.LoadScene("Sumas_y_restas_medio_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_2") SceneManager.LoadScene("Sumas_y_restas_medio_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_3") SceneManager.LoadScene("Sumas_y_restas_medio_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_4") SceneManager.LoadScene("Sumas_y_restas_medio_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_5")  // DIFICIL NIVEL 1
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0, (int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
            }
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_1") SceneManager.LoadScene("Sumas_y_restas_dificil_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_2") SceneManager.LoadScene("Sumas_y_restas_dificil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_3") SceneManager.LoadScene("Sumas_y_restas_dificil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_4") SceneManager.LoadScene("Sumas_y_restas_dificil_5", LoadSceneMode.Single);
            else if(Application.loadedLevelName == "Sumas_y_restas_dificil_5")
            {
                actJoyStick.SetActive(false);
                actJump.SetActive(false);
                loadDataContadores();
                scoreText.text = Puntaje.puntaje.puntos.ToString();
                timeText.text = TimeController.timeController.minSeg;;

                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);

                //Guardado de resultado
                ResultsManager.instance.AddResult(System.DateTime.Now.ToString(), 0, 0, (int)Puntaje.puntaje.puntos, (int)TimeController.timeController.restante, precision);

                // TODO: APAGAR TODO EL SISTEMA DE CONTEO, PUNTAJE Y CONTADORES
                TimeController.timeController.IsGameOver();
                TimeController.timeController.IsOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                Puntaje.puntaje.IsOver();
                CoinContController.coinContController.IsOver();

                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
                ContController.setContSuccess(0);
                ContController.setContFailure(0);
                resultPanel.SetActive(true);
            }
           
            
        }
        
    }


}
