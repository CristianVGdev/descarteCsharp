using System;
using System.Collections.Generic;
using UnrealEngine.Core;
using UnrealEngine.Engine;
using UnrealEngine.InputCore;
using UnrealEngine.Runtime;

public class MyPlayerCharacter : Character
{
    public delegate void DeathEventDelegate();
    public static event DeathEventDelegate DeathEvent;

    private bool isPlayerDead = false;
    private bool isReviving = false;
    private int reviveCount = 0;
    private Widget reviveScreen;
    private Random random = new Random();
    private const float maxStamina = 100f;
    private float currentStamina = maxStamina;
    private AnimationInstance deathAnimation;
    private AnimationInstance surrenderAnimation;

    public override void BeginPlay()
    {
        base.BeginPlay();

        // Cargar y asignar la animación de muerte
        deathAnimation = new AnimationInstance("/Game/Animations/DeathAnimation");
        Mesh.SetAnimationInstance(deathAnimation);
        deathAnimation.AnimationFinishedEvent += OnDeathAnimationFinished;

        // Cargar y asignar la animación de rendición
        surrenderAnimation = new AnimationInstance("/Game/Animations/SurrenderAnimation");
        surrenderAnimation.AnimationFinishedEvent += OnSurrenderAnimationFinished;

        // Suscribirse al evento de muerte del personaje
        DeathEvent += OnPlayerDeath;

        // Cargar y mostrar la pantalla de revivir
        reviveScreen = WidgetBlueprint.Load("/Game/UI/ReviveScreen");
        reviveScreen.AddToViewport(0);
        reviveScreen.Visibility = Visibility.Hidden;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        // Verificar si el personaje está muerto y mostrar la pantalla de revivir
        if (isPlayerDead && !isReviving && reviveCount < 3)
        {
            reviveScreen.Visibility = Visibility.Visible;

            // Verificar si se pulsa el botón de revivir
            if (Input.GetKey(EKeys.Enter) && currentStamina >= 10f)
            {
                isReviving = true;
                RevivePlayer();
            }
        }
    }

    private void OnPlayerDeath()
    {
        if (!isPlayerDead)
        {
            isPlayerDead = true;
            currentStamina -= currentStamina * 0.1f; // Pérdida del 10% de la estamina al morir

            if (reviveCount >= 2)
            {
                GameOver();
            }
            else
            {
                // Reproducir la animación de muerte
                deathAnimation.Play();
            }
        }
    }

    private void OnDeathAnimationFinished()
    {
        // Aquí puedes realizar acciones adicionales después de que la animación de muerte haya terminado
    }

    private void RevivePlayer()
    {
        // Generar un punto aleatorio en el mapa
        FVector randomLocation = new FVector(random.Next(-1000, 1000), random.Next(-1000, 1000), 0);

        // Colocar al personaje en el punto aleatorio
        SetActorLocation(randomLocation);

        // Reiniciar los valores de muerte y revivir
        isPlayerDead = false;
        isReviving = false;
        reviveCount++;

        // Ocultar la pantalla de revivir
        reviveScreen.Visibility = Visibility.Hidden;
    }

    private void GameOver()
    {
        // Reproducir la animación de rendición
        surrenderAnimation.Play();

        // Realizar las acciones necesarias para el game over
        // ...
    }

    private void OnSurrenderAnimationFinished()
    {
        // Aquí puedes realizar acciones adicionales después de que la animación de rendición haya terminado
    }
}
