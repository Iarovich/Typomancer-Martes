using System.Collections.Generic;
using UnityEngine;
using System;

//Event Queue - procesa eventos de forma asíncrona y ordenada
public class EventQueue : MonoBehaviour
{
    //Singleton
    public static EventQueue Instance { get; private set; }

    //Cola de eventos pendientes
    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();
    
    //Evento actual siendo procesado
    private GameEvent currentEvent = null;
    
    //Tiempo entre procesamiento de eventos (en segundos)
    [SerializeField] private float eventProcessingDelay = 0.1f;
    private float timeSinceLastEvent = 0f;
    
    //Indica si hay un evento siendo procesado
    private bool isProcessingEvent = false;

    void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        //Procesa eventos de la cola
        ProcessEventQueue();
    }

    //Event Queue - Encola un evento para procesamiento posterior
    public void EnqueueEvent(GameEvent gameEvent)
    {
        if (gameEvent == null) return;
        eventQueue.Enqueue(gameEvent);
        Debug.Log($"[EventQueue] Evento encolado: {gameEvent.EventType}. Cola: {eventQueue.Count}");
    }

    //Event Queue - Procesa eventos de la cola uno por uno
    private void ProcessEventQueue()
    {
        //Si hay un evento siendo procesado, esperar
        if (isProcessingEvent)
        {
            timeSinceLastEvent += Time.deltaTime;
            if (timeSinceLastEvent >= eventProcessingDelay)
            {
                isProcessingEvent = false;
                timeSinceLastEvent = 0f;
            }
            return;
        }

        //Si no hay eventos en la cola, salir
        if (eventQueue.Count == 0) return;

        //Procesar el siguiente evento
        currentEvent = eventQueue.Dequeue();
        isProcessingEvent = true;
        timeSinceLastEvent = 0f;

        Debug.Log($"[EventQueue] Procesando evento: {currentEvent.EventType}");

        //Ejecutar el evento
        currentEvent.Execute();
        
        //El evento se marcará como completado cuando termine su ejecución
    }

    //Marca el evento actual como completado
    public void CompleteCurrentEvent()
    {
        if (currentEvent != null)
        {
            Debug.Log($"[EventQueue] Evento completado: {currentEvent.EventType}");
            currentEvent = null;
        }
        isProcessingEvent = false;
    }

    //Limpia la cola de eventos
    public void ClearQueue()
    {
        eventQueue.Clear();
        currentEvent = null;
        isProcessingEvent = false;
        Debug.Log("[EventQueue] Cola limpiada");
    }

    public int QueueCount => eventQueue.Count;
    public bool IsProcessing => isProcessingEvent;
}

//Event Queue - Clase base para eventos del juego
public abstract class GameEvent
{
    public abstract string EventType { get; }
    public abstract void Execute();
}

//Event Queue - Evento de activación de carta
public class CardActivationEvent : GameEvent
{
    public override string EventType => "CardActivation";
    
    private CardData cardData;
    private CardEffectHandler effectHandler;
    private HandManager handManager;
    private CardView cardView;

    public CardActivationEvent(CardData card, CardEffectHandler handler, HandManager handMgr, CardView view)
    {
        cardData = card;
        effectHandler = handler;
        handManager = handMgr;
        cardView = view;
    }

    public override void Execute()
    {
        if (cardData == null || effectHandler == null) return;

        //Aplicar efecto de la carta
        effectHandler.ApplyEffect(cardData);
        
        //Usar la carta
        if (handManager != null && cardView != null)
        {
            handManager.UseCard(cardView);
        }

        //Regla especial para DARK
        if (cardData.element == CardElement.Dark && handManager != null)
        {
            handManager.DiscardRandomFromHand();
        }

        //Marcar evento como completado
        EventQueue.Instance?.CompleteCurrentEvent();
    }
}

//Event Queue - Evento de efecto VFX
public class VFXEvent : GameEvent
{
    public override string EventType => "VFX";
    
    private CardData cardData;
    private CardEffectHandler effectHandler;

    public VFXEvent(CardData card, CardEffectHandler handler)
    {
        cardData = card;
        effectHandler = handler;
    }

    public override void Execute()
    {
        if (cardData == null || effectHandler == null) return;

        //Spawnear efecto visual
        effectHandler.SpawnEffect(cardData);

        //Marcar evento como completado después de un delay
        if (EventQueue.Instance != null)
        {
            EventQueue.Instance.StartCoroutine(CompleteAfterDelay(1f));
        }
    }

    private System.Collections.IEnumerator CompleteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventQueue.Instance?.CompleteCurrentEvent();
    }
}

//Event Queue - Evento de daño
public class DamageEvent : GameEvent
{
    public override string EventType => "Damage";
    
    private Health target;
    private int damageAmount;

    public DamageEvent(Health targetHealth, int damage)
    {
        target = targetHealth;
        damageAmount = damage;
    }

    public override void Execute()
    {
        if (target != null)
        {
            target.TakeDamage(damageAmount);
        }
        EventQueue.Instance?.CompleteCurrentEvent();
    }
}

