﻿// Rory Clark - https://rory.games - 2019

using System;
using Gyro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Player : MonoBehaviour
    {
        // Objects for movement:
        private const float MovementRate = 700f;
        private PFI_SpaceInvaders_Controller _controlsScript;
        private static Quaternion OriginalRot => Quaternion.Euler(
            55,
            90,
            0);
        
        private Vector2 _moveData;
        public Vector2 MoveData => _moveData;
        
        // Gyro Controls Variables:
        private Gamepad _controller;
        private Transform _transform;
        private Rigidbody _rigidbody;


        private void Awake() {
            
            _controlsScript = new PFI_SpaceInvaders_Controller();
            _controller = DS4.GetController();
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            
            
            // Link up data from controller to a variable (Movement):
            _controlsScript.Player.Move.performed += context => _moveData = context.ReadValue<Vector2>();
            _controlsScript.Player.Move.canceled += context => _moveData = context.ReadValue<Vector2>();
        }

        private void Update() {
            
            // Gyro Movement:
            // Check overridden control layer is initialised:
            if (_controller == null) {
                try {
                    _controller = DS4.GetController();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            else {
                // Get data from gyro:
                var gyroZData = DS4.ProcessRawData(DS4.gyroZ.ReadValue());
                _transform.position += (new Vector3(0f, 0f, -gyroZData * MovementRate * Time.deltaTime));
            }
            
            // Calculate new movement and apply it to player rigidbody component:
            _rigidbody.AddForce(new Vector3(0f, 0f, -_moveData.x * MovementRate * Time.deltaTime));
            
            // Rotate player based on movement:
            transform.Rotate(Vector3.up * (_moveData.x * 50f * Time.deltaTime));
            
            // If player stops moving - return to original rotation:
            if (_moveData != Vector2.zero) return;
            var rotation = transform.rotation;
            rotation = Quaternion.Lerp(Quaternion.Euler(
                    rotation.eulerAngles.x,
                    rotation.eulerAngles.y,
                    rotation.eulerAngles.z),
                OriginalRot, 
                Time.deltaTime);
            transform.rotation = rotation;
            
        }

        private void OnEnable() {
            _controlsScript.Player.Enable();
        }

        private void OnDisable() {
            _controlsScript.Player.Disable();
        }
    }
}

