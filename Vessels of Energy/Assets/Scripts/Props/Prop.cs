﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Token {
    [Header("Prop")]
    public PropProperty properties;

    public override void Animate(string command) {
        switch (command) {
            case "destroy":
                OnRemoved();
                this.gameObject.SetActive(false);
                break;
        }
    }

    public virtual void OnRemoved() { }
}
