﻿using System;
using UnityEngine;

namespace Asteroids {
    public class Asteroid {
        private float _health;
        public float Health => _health;
        
        private float _velocity;
        public float Velocity => _velocity;
        
        private float _spawnRate;
        public float SpawnRate => _spawnRate;

        private Vector3 _scale;
        public Vector3 Scale => _scale;

        private int _scoreValue;
        public int ScoreValue => _scoreValue;

        private AsteroidType _asteroidType;
        public AsteroidType AsteroidType => _asteroidType;

        private GameObject _asteroidPrefab;
        public GameObject AsteroidPrefab => _asteroidPrefab;
    
        // Constructor:
        public Asteroid(AsteroidType asteroidType) {
            
            // Assign Asteroid Prefab:
            _asteroidPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
            
            // Assign all the asteroids properties on initialisation based on an enum input param.
            switch (asteroidType) {
                case AsteroidType.A1:
                    _asteroidType = AsteroidType.A1;
                    _health = 2f;
                    _velocity = 100f;
                    _spawnRate = 20f;
                    _scale = new Vector3(1f, 1f, 1f);
                    _scoreValue = 50;
                    break;
                case AsteroidType.A2:
                    _asteroidType = AsteroidType.A2;
                    _health = 6f;
                    _velocity = 80f;
                    _spawnRate = 25f;
                    _scale = new Vector3(1.5f, 1.5f, 1.5f);
                    _scoreValue = 30;
                    break;
                case AsteroidType.A3:
                    _asteroidType = AsteroidType.A3;
                    _health = 10f;
                    _velocity = 70f;
                    _spawnRate = 30f;
                    _scale = new Vector3(2f, 2f, 2f);
                    _scoreValue = 30;
                    break;
                case AsteroidType.A4:
                    _asteroidType = AsteroidType.A4;
                    _health = 15f;
                    _velocity = 60f;
                    _spawnRate = 15f;
                    _scale = new Vector3(2.5f, 2.5f, 2.5f);
                    _scoreValue = 40;
                    break;
                case AsteroidType.A5:
                    _asteroidType = AsteroidType.A5;
                    _health = 20f;
                    _velocity = 50f;
                    _spawnRate = 10f;
                    _scale = new Vector3(3f, 3f, 3f);
                    _scoreValue = 50;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(asteroidType), asteroidType, null);
            }
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
