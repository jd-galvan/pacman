using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
  [SerializeField]
  private AnimatedSprite deathSequence;
  private SpriteRenderer spriteRenderer;
  private CircleCollider2D circleCollider;
  private Movement movement;

  public IInputHandler inputHandler; // Interfaz para manejar entradas

  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    circleCollider = GetComponent<CircleCollider2D>();
    movement = GetComponent<Movement>();
    inputHandler = GetComponent<IInputHandler>();
  }

  private void Update()
  {
    if (inputHandler != null)
    {
      Vector2 newDirection = inputHandler.GetInputDirection();
      if (newDirection != Vector2.zero)
      {
        movement.SetDirection(newDirection);

        // Rotar a Pacman en la dirección del movimiento
        float angle = Mathf.Atan2(newDirection.y, newDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
      }
    }
  }

  public void ResetState()
  {
    enabled = true;
    spriteRenderer.enabled = true;
    circleCollider.enabled = true;
    Debug.Log("ENTRO AQUI");
    Debug.Log(deathSequence);
    deathSequence.enabled = false;
    movement.ResetState();
    gameObject.SetActive(true);
  }

  public void DeathSequence()
  {
    enabled = false;
    spriteRenderer.enabled = false;
    circleCollider.enabled = false;
    movement.enabled = false;
    deathSequence.enabled = true;
    deathSequence.Restart();
  }
}
