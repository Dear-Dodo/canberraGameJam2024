using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;

namespace Player
{
    [CustomEditor(typeof(Player))]
    public class PlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Player Target = target as Player;

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Damage 10"))
                {
                    Target.Damage(10f);
                }

                if (GUILayout.Button("Heal 10"))
                {
                    Target.Heal(10f);
                }
            }
        }
    }

}
#endif

namespace Player
{

    public class Player : MonoBehaviour
    {
        public float Health { get { return _health; } }

        public float MaxHealth;
        public float MoveSpeed;
        public float DashDistance;
        public float DashTime;
        public float DashCooldown;
        public float DashLongCooldown;
        public int DashCharges;
        public float ReloadTime;
        public float LaserReloadTime;
        public bool HasLaser;

        public PlayerHealthBar HealthBar;
        public PlayerDashCounter DashCounter;

        public SparkleBullet SparklePrefab;
        public GameObject Laser;

        [HideInInspector]
        public bool IsDashing = false; //will use this for Iframes


        private float _health;
        private bool _canDash = true;
        private bool _canFire = true;
        private float _dashCharges;

        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _fireAction;

        // Start is called before the first frame update
        void Start()
        {
            _health = MaxHealth;

            _moveAction = InputSystem.actions.FindAction("Move");
            _dashAction = InputSystem.actions.FindAction("Dash");
            _fireAction = InputSystem.actions.FindAction("Fire");

            _dashCharges = DashCharges;
            DashCounter.Redraw((int)_dashCharges);
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            moveValue = moveValue.normalized * Mathf.Clamp01(moveValue.magnitude); //Fix diagonals being faster on keyboard

            bool dashValue = _dashAction.WasPressedThisFrame();

            bool fireValue = _fireAction.IsPressed();

            transform.position += new Vector3(moveValue.x, moveValue.y) * MoveSpeed;
            if (dashValue && _canDash && _dashCharges > 0)
            {
                StartCoroutine(Dash(moveValue));
            }

            if(fireValue && _canFire)
            {
                StartCoroutine(Fire());
            }

            Laser.SetActive(HasLaser && fireValue);

            
        }

        IEnumerator Fire()
        {
            _canFire = false;
            float offset = Random.Range(-0.5f, 0.5f);
            SparkleBullet sparkleBullet = Instantiate(SparklePrefab, transform.position + new Vector3(offset,0), Quaternion.identity);
            if (HasLaser)
            {
                sparkleBullet.Offset = offset;
                sparkleBullet.LaserMode = true;
                sparkleBullet.Player = gameObject;
                yield return new WaitForSeconds(LaserReloadTime);
            }
            else
            {
                yield return new WaitForSeconds(ReloadTime);
            }
            _canFire = true;
        }

        IEnumerator Dash(Vector2 moveValue)
        {
            Vector3 dashStart = transform.position;
            Vector3 dashTarget = transform.position + new Vector3(moveValue.x, moveValue.y) * DashDistance;
            float timer = 0;
            _canDash = false;
            IsDashing = true;
            _dashCharges--;
            DashCounter.Redraw((int)_dashCharges);

            while (timer < DashTime)
            {
                transform.position = Vector3.Lerp(dashStart,dashTarget,timer / DashTime);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            IsDashing = false;

            StartCoroutine(DoDashCooldown());
            yield return new WaitForSeconds(DashLongCooldown);
            _dashCharges++;
            DashCounter.Redraw((int)_dashCharges);
        }

        //probably a better way to do this but it works
        IEnumerator DoDashCooldown()
        {
            yield return new WaitForSeconds(DashCooldown);
            _canDash = true;
        }

        public void Damage(float damage)
        {
            if (!IsDashing)
            {
                _health -= damage;

                HealthBar.SetHealth(Health, MaxHealth);

                if (_health <= 0)
                {
                    _health = 0;
                    //die
                    Debug.Log("you are dead");
                }
            }
        }

        public void Heal(float healing)
        {
            _health += healing;
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
            HealthBar.SetHealth(Health, MaxHealth);
        }
    }
}
