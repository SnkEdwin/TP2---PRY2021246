using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    
    [SerializeField] public bool comenzo;
    //private string comenzoPrefsName = "comenzó";
    void Awake()
    {
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        loadData();
        print("contadorFallido: " + contadorFallido);
        print("contadorAciertos: " + contadorAciertos);
    }

    // Start is called before the first frame update
    void Start()    
    {
      
        startPosition = this.transform.position;
       //if (comenzo)
       //{
       //    
       //    //comenzo=false;
       //    //contadorAciertos = 0;
       //    //contadorFallido = 0;
       //    //layerPrefs.SetFloat(counterFailurePrefsName,contadorFallido);
       //    //layerPrefs.SetFloat(counterSuccessPrefsName,contadorAciertos);
       //    
       //}
        
        //timeController = GameObject.Find("TimeController").GetComponent<TimeController>();

    }

    public void StartGame()
    {   //Cuando empezamos a jugar el jugador no estar� en el suelo
        animator.SetBool(STATE_ON_THE_GROUND, false);
        animator.SetBool(STATE_IS_WALKING, false); 
        this.transform.position = startPosition;
        //Esto se pone para que la velocidad sea igual a la de la inicial
        this.rigidBody.velocity = Vector2.zero;
    }
    
   //public void saveData(float _contadorFallidos,float _contadorAciertos)
   //{
   //    contadorFallido = _contadorFallidos;
   //    contadorAciertos = _contadorAciertos;
   //    PlayerPrefs.SetFloat(counterFailurePrefsName,contadorFallido);
   //    PlayerPrefs.SetFloat(counterSuccessPrefsName,contadorAciertos);
   //    //PlayerPrefs.SetString(comenzoPrefsName,comenzo.ToString());
   //}
    
    
    private void loadData()
    {
        contadorFallido = ContController.getContFailure();
        contadorAciertos = ContController.getContSuccess();
        //comenzo = Convert.ToBoolean(PlayerPrefs.GetString(comenzoPrefsName, "True"));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{   
//
        //    Jump();
        //}
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
        //Debug.DrawRay(this.transform.position, Vector2.down * distanceRay, Color.red);
    }

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
        
        //Debug.Log("joystick"+joystick.Horizontal);

        
        //if (expr)
        //{
        //    animator.SetBool(STATE_IS_WALKING, false);
        //}
        //
        //if (Input.GetKey(KeyCode.RightArrow))
        //{   //Para la direccion de la animacion
        //    rigidBody.GetComponent<SpriteRenderer>().flipX = false;
        //    //Para la velocidad de movimiento en X
        //    rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
        //    //Para activar el estado STATE_IS_WALKING
        //    animator.SetBool(STATE_IS_WALKING, true);
//
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{   rigidBody.GetComponent<SpriteRenderer>().flipX = true;
        //    rigidBody.velocity = new Vector2(-runningSpeed, rigidBody.velocity.y);
        //    animator.SetBool(STATE_IS_WALKING, true);
        //}
        //else
        //{   //Poner en estado off la animaci�n de caminar
        //    animator.SetBool(STATE_IS_WALKING, false);
        //    //Evitar deslizamiento cuando se desactive la fricci�n  
        //    rigidBody.velocity = new Vector2(runningSpeed * 0, rigidBody.velocity.y);
        //}


    }

    // Es un m�todo que si o si se va a utilizar
    public void Jump()
    {
        animator.SetBool(STATE_IS_WALKING, false);

        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
        }

    
    }
    // Para esto debo crear un nuevo Layer llamado Ground para todas las plataformas
    //Esto nos va a servir para que el salto no se haga consecutivo
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
    
    private void OnApplicationQuit()
    {
        
        ContController.setContFailure(0);
        ContController.setContSuccess(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Incorrect"))
        {
            contadorFallido += 1;
            ContController.setContFailure(contadorFallido);
            //print("contadorFallido: " + contadorFallido);
            print("contadorFailure: " + ContController.getContFailure());
        }
        else if (collision.CompareTag("Correct"))
        {
            contadorAciertos += 1;
            ContController.setContSuccess(contadorAciertos);
            print("contadorAciertos: " + ContController.getContSuccess());
            
            // FACIL NIVEL 1
            if (Application.loadedLevelName == "Sumas_y_restas_facil_1") SceneManager.LoadScene("Sumas_y_restas_facil_2", LoadSceneMode.Single); 
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_2") SceneManager.LoadScene("Sumas_y_restas_facil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_3") SceneManager.LoadScene("Sumas_y_restas_facil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_4") SceneManager.LoadScene("Sumas_y_restas_facil_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_facil_5") //MEDIO NIVEL 1
            {
                SceneManager.LoadScene("Sumas_y_restas_medio_1", LoadSceneMode.Single);
            }
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_1") SceneManager.LoadScene("Sumas_y_restas_medio_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_2") SceneManager.LoadScene("Sumas_y_restas_medio_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_3") SceneManager.LoadScene("Sumas_y_restas_medio_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_4") SceneManager.LoadScene("Sumas_y_restas_medio_5", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_medio_5")  // DIFICIL NIVEL 1
            {
                SceneManager.LoadScene("Sumas_y_restas_dificil_1", LoadSceneMode.Single); 
            }
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_1") SceneManager.LoadScene("Sumas_y_restas_dificil_2", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_2") SceneManager.LoadScene("Sumas_y_restas_dificil_3", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_3") SceneManager.LoadScene("Sumas_y_restas_dificil_4", LoadSceneMode.Single);
            else if (Application.loadedLevelName == "Sumas_y_restas_dificil_4") SceneManager.LoadScene("Sumas_y_restas_dificil_5", LoadSceneMode.Single);
            else
            {
                loadData();
                TimeController.timeController.IsGameOver();
                Puntaje.puntaje.IsGameOver();
                CoinContController.coinContController.IsGameOver();
                //TimeController.timeController.IsOver();
                //Puntaje.puntaje.IsOver();
                //CoinContController.coinContController.IsOver();
                contadorTotal = contadorAciertos + contadorFallido;
                precision = (contadorAciertos / contadorTotal);
                Debug.Log("Colision - Gano el juego");
                Debug.Log("N° Total Incorrectas: "+ContController.getContFailure());
                Debug.Log("N° de Precisión del nivel: "+decimal.Round((decimal)precision*100,2));
            }
           
            
        }
        else if (collision.CompareTag("Block"))
        {
            
            transform.position += new Vector3(0, 0,0) * 0 * 0;
        }
    }


}
