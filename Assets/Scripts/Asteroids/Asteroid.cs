﻿using System;
using Camera;
using Game;
using Player;
using Sound;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Asteroids {
    public class Asteroid : MonoBehaviour {

        private float CurrentHealth { get; set; }
        
        [SerializeField] private float totalHealth;
        public float TotalHealth => totalHealth;
        
        [SerializeField] private float velocity;
        public float Velocity => velocity;

        [SerializeField] private Vector3 scale;
        public Vector3 Scale => scale;

        [SerializeField] private int scoreValue;
        public int ScoreValue => scoreValue;

        [SerializeField] private AsteroidType asteroidType;
        public AsteroidType AsteroidType => asteroidType;
        
        [SerializeField] private int damageToPlayer;
        public int DamageToPlayer => damageToPlayer;

        private Rigidbody _asteroidRigidbody;
        [SerializeField]
        private float time; 
        private float _currentScale;
        private bool _finishedScaling;
        private GameObject _explosionPrefab;
        private GameObject _explosionPrefab2;
        private GameObject _smokePrefab;

        // Constructor
        public Asteroid(AsteroidType asteroidTypeParam) {
            asteroidType = asteroidTypeParam;
        }
        
        private void Awake() {

            const int a1SpawnRate = 10,
                a2SpawnRate = 35,
                a3SpawnRate = 75,
                a4SpawnRate = 90,
                a5SpawnRate = 101;
            
            // Assign asteroid type according to spawn rate:
            var randNumb = Random.Range(0, 100);
            if (randNumb < a1SpawnRate) {
                asteroidType = AsteroidType.A1;
            }
            else if (randNumb < a2SpawnRate) {
                asteroidType = AsteroidType.A2;
            }
            else if (randNumb < a3SpawnRate) {
                asteroidType = AsteroidType.A3;
            }
            else if (randNumb < a4SpawnRate) {
                asteroidType = AsteroidType.A4;
            }
            else if (randNumb < a5SpawnRate) {
                asteroidType = AsteroidType.A5;
            }
            
            // Assign all the asteroids properties on initialisation based on an enum input param.
            switch (asteroidType) {
                case AsteroidType.A1:
                    asteroidType = AsteroidType.A1;
                    totalHealth = 1f;
                    CurrentHealth = 1f;
                    velocity = 25f;
                    scale = new Vector3(1f, 1f, 1f);
                    scoreValue = 5;
                    damageToPlayer = 1;
                    break;
                case AsteroidType.A2:
                    asteroidType = AsteroidType.A2;
                    totalHealth = 2f;
                    CurrentHealth = 2f;
                    velocity = 20f;
                    scale = new Vector3(1.5f, 1.5f, 1.5f);
                    scoreValue = 10;
                    damageToPlayer = 3;
                    break;
                case AsteroidType.A3:
                    asteroidType = AsteroidType.A3;
                    totalHealth = 2f;
                    CurrentHealth = 2f;
                    velocity = 15f;
                    scale = new Vector3(2f, 2f, 2f);
                    scoreValue = 20;
                    damageToPlayer = 5;
                    break;
                case AsteroidType.A4:
                    asteroidType = AsteroidType.A4;
                    totalHealth = 4f;
                    CurrentHealth = 4f;
                    velocity = 10f;
                    scale = new Vector3(2.5f, 2.5f, 2.5f);
                    scoreValue = 30;
                    damageToPlayer = 6;
                    break;
                case AsteroidType.A5:
                    asteroidType = AsteroidType.A5;
                    totalHealth = 6f;
                    CurrentHealth = 6f;
                    velocity = 2f;
                    scale = new Vector3(8f, 8f, 8f);
                    scoreValue = 50;
                    damageToPlayer = 15;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(asteroidType), asteroidType, null);
            }
            
            // Set asteroid scale to (0,0,0):
            transform.localScale = Vector3.zero;
            
            // Add torque:
            _asteroidRigidbody = GetComponent<Rigidbody>();
            AddRandomTorque(_asteroidRigidbody);
            
            _explosionPrefab = Resources.Load<GameObject>("Prefabs/ParticleFX/Explosion");
            _explosionPrefab2 = Resources.Load<GameObject>("Prefabs/ParticleFX/Explosion_2");
            _smokePrefab = Resources.Load<GameObject>("Prefabs/ParticleFX/Smoke");
        }

        private void Update() {
            
            ClampVelocity(Velocity, _asteroidRigidbody);
            OutOfBoundsCheck(transform.position.x, -20f);
            
            if(!_finishedScaling)
                // Scale on spawn from (0,0,0) to the set scale:
                ScaleOnSpawn(ref time, ref _finishedScaling, 1f, Scale.x);
        }

        private static void AddRandomTorque(Rigidbody rigidbody) {
            
            const float minRandomTorque = -50; 
            const float maxRandomTorque = 50;
            rigidbody.AddRelativeTorque(new Vector3(Random.Range(minRandomTorque, maxRandomTorque), 
                Random.Range(minRandomTorque, maxRandomTorque), Random.Range(minRandomTorque, 
                    maxRandomTorque)));
        }
        
        private static void ClampVelocity(float maxVelocity, Rigidbody rigidbody) {
            
            if (rigidbody.velocity.magnitude > maxVelocity) {
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
            }
        }
        
        private void OutOfBoundsCheck(float xPos, float threshold) {
            if (!(xPos <= threshold)) return;
            PlayerHealth.DamagePlayer(damageToPlayer);
            Destroy(gameObject);
        }
        
        private void ScaleOnSpawn(ref float timeParam, ref bool isFullScale, float lerpTime, float desiredScaleParam) {
            
            // Scale on spawn from (0,0,0) to the set scale:
            timeParam += Time.deltaTime;

            if(timeParam > lerpTime)
            {
                this.time = lerpTime;
            }
            
            // Lerp between 0 and 2:
            _currentScale = Mathf.Lerp(0, desiredScaleParam, time/lerpTime);
            // Set currentScale to the localScale:
            transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);

            // Check if the cube has finished scaling:
            if (transform.localScale.x > desiredScaleParam) {
                isFullScale = true;
                
            }
        }
        
        public void DamageAsteroid(float damage)
        {
            CurrentHealth -= damage;
            
            // Change color depending on asteroid current HP
            // Red == High HP, Black == Low HP:
            gameObject.GetComponent<Renderer>().material.color *= Utility.PercentageFunc(CurrentHealth, TotalHealth);


            if (!(CurrentHealth <= 0)) return;
            // Asteroid's HP is 0:
            // Add asteroid score to total score:
            GameController.AddScore(ScoreValue);
            
            // Large asteroid:
            if (AsteroidType == AsteroidType.A5) {
                if (!(UnityEngine.Camera.main is null))
                    UnityEngine.Camera.main.GetComponent<CameraShake>().StartShake(0.2f, 0.6f);
                Utility.ActivateSleep(0.015f);  
                SpawnPrefab(_smokePrefab);
                SpawnPrefab(_explosionPrefab2);
                SoundEffects.PlaySfx(SoundEffects.SoundEffectID.LargeAsteroidExplosion);
            }
            // Other asteroids:
            else {
                if (Random.Range(0, 100) > 90) {
                    Utility.ActivateSleep(0.015f);  
                    SpawnPrefab(_smokePrefab);
                    SpawnPrefab(_explosionPrefab2);
                    SoundEffects.PlaySfx(SoundEffects.SoundEffectID.AsteroidExplosion);
                }
                else {
                    Utility.ActivateSleep(0.005f);  
                    SpawnPrefab(_explosionPrefab);   
                    SoundEffects.PlaySfx(SoundEffects.SoundEffectID.AsteroidExplosion);
                }
                
            }
            Destroy(gameObject);
        }
        
        private void SpawnPrefab(GameObject prefab) {
            var explosion = Instantiate(prefab);
            explosion.transform.localPosition = transform.position;
            explosion.transform.localScale = Scale / 2;
        }
    }

    public enum AsteroidType {
        A1,
        A2,
        A3,
        A4,
        A5
    };
    
}
