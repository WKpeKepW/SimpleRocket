using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    enum State { Alive, Dead, Win, WinEnd };
    State state;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip DeadSound;
    [SerializeField] AudioClip WinSound;
    [SerializeField] ParticleSystem flyPartical;
    [SerializeField] ParticleSystem DeadPartical;
    [SerializeField] ParticleSystem WinPartical;
    [SerializeField] Camera cam;
    [SerializeField] Canvas winscrin;
    [SerializeField] float SpeedRotation = 200f;
    [SerializeField] float SpeedLaunch = 700f;

    Rigidbody rb;
    AudioSource audioSrc;


    void Start()
    {
        state = State.Alive;
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
            Move();
        MoveCamera();
        if (state == State.WinEnd)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && state != State.WinEnd)
            Application.Quit();
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * SpeedLaunch * Time.deltaTime);

            audioSrc.PlayOneShot(flySound);
            if (!flyPartical.isPlaying)
                flyPartical.Play();
        }
        else
        {
            audioSrc.Stop();
            if (flyPartical.isPlaying)
                flyPartical.Stop();
        }

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(-1 * SpeedRotation * Time.deltaTime, 0, 0));

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(1 * SpeedRotation * Time.deltaTime, 0, 0));
    }
    void MoveCamera()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            cam.transform.position = new Vector3(cam.transform.position.x, transform.position.y + 6, transform.position.z);

        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            //для первого левела 
            if (transform.position.y < 10)
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
            else
                cam.transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || state == State.WinEnd)
            return;
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                break;
            case "Finish":
                Win();
                break;
            default:
                Dead();
                break;
        }
    }
    void Win()
    {
        state = State.Win;
        audioSrc.Stop();
        WinPartical.Play();
        flyPartical.Stop();
        audioSrc.PlayOneShot(WinSound);
        if (SceneManager.GetActiveScene().buildIndex == 1)
            Invoke("LoadSecondLevel", 2f);
        if (SceneManager.GetActiveScene().buildIndex == 2)
            Invoke("LoadThirdLevel", 2f);
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            winscrin.gameObject.SetActive(true);
            state = State.WinEnd;
        } 
    }
    public void Dead()
    {
        state = State.Dead;
        audioSrc.Stop();
        DeadPartical.Play();
        flyPartical.Stop();
        audioSrc.PlayOneShot(DeadSound);
        Invoke("LoadFirstLevel", 2f);
    }
    void LoadThirdLevel() => SceneManager.LoadScene(3);
    void LoadSecondLevel() => SceneManager.LoadScene(2);
    void LoadFirstLevel() => SceneManager.LoadScene(1);
}
