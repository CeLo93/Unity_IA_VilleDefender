using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    #region --------------------VARIAVEIS--------------------
    // Configurações do NPC
    [Header("Configurações do NPC")]
    public Transform[] waypoints; // Array para armazenar os waypoints da vila
    private int currentWaypointIndex = 0; // Índice do waypoint atual
    public float timeToRest = 1f; // Tempo de descanso após batalhas

    // Necessidades dos NPCs
    [Header("Necessidades dos NPCs")]
    private bool isHungry = false;
    private bool isThirsty = false;
    private bool isTired = false;

    // Variáveis para controle interno
    private bool isMoving = false;
    private bool isWaiting = false;
    private bool isRecovering = false;

    // Raio para detecção de obstáculos
    [Header("Detecção de Obstáculos")]
    public float obstacleDetectionRadius = 1f;
    public LayerMask obstacleLayerMask; // Camada de obstáculos

    private NavMeshAgent agent;

    // Tempo de espera no waypoint
    [Header("Tempo de Espera no Waypoint")]
    public float waitTimeAtWaypoint;
    #endregion --------------------VARIAVEIS--------------------

    #region --------------------INICIALIZAR--------------------
    private void Start()
    {
        // Obter o componente NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion --------------------INICIALIZAR--------------------

    #region --------------------ATUALIZAR--------------------
    private void Update()
    {
        if (!isMoving && !isWaiting && !isRecovering)
        {
            // Inicia a rotina para se mover para o próximo waypoint
            StartCoroutine(MoveToNextWaypoint());
        }

        // Aqui você pode adicionar outras lógicas específicas do seu jogo, como verificar fome, sede, etc.
    }
    #endregion --------------------ATUALIZAR--------------------

    #region --------------------CORROTINAS--------------------
    // Função para mover o NPC até o próximo waypoint
    private IEnumerator MoveToNextWaypoint()
    {
        // Verificar se há waypoints definidos
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Nenhum waypoint definido para o NPC " + gameObject.name);
            yield break;
        }

        // Definir o destino como o próximo waypoint
        Vector3 targetWaypoint = waypoints[currentWaypointIndex].position;

        // Movimentação do NPC até o waypoint
        agent.SetDestination(targetWaypoint);
        isMoving = true;

        // Aguardar até o NPC alcançar o waypoint
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            // Verificar se há obstáculos à frente usando Raycast
            Vector3 direction = targetWaypoint - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, obstacleDetectionRadius, obstacleLayerMask))
            {
                // Desviar do obstáculo
                Vector3 avoidDirection = Vector3.Cross(Vector3.up, direction.normalized);
                Vector3 avoidPosition = hit.point + avoidDirection * obstacleDetectionRadius;

                // Encontrar uma posição válida usando NavMesh
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(avoidPosition, out navHit, obstacleDetectionRadius, NavMesh.AllAreas))
                {
                    targetWaypoint = navHit.position;
                }
            }

            yield return null;
        }

        // Chegou ao waypoint, vamos para o próximo
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isMoving = false;

        // Calcular a duração do tempo de espera proporcional à distância entre os waypoints e a velocidade do NavMeshAgent
        float distanceToNextWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);
        float waitDuration = waitTimeAtWaypoint;

        // Esperar antes de se mover para o próximo waypoint
        StartCoroutine(Wait(waitDuration));
    }

    // Função para esperar por um tempo específico
    private IEnumerator Wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;
    }

    // Função para recuperar o NPC após batalhas
    private IEnumerator RecoverFromBattle()
    {
        isRecovering = true;
        yield return new WaitForSeconds(timeToRest);
        isRecovering = false;
    }
    #endregion --------------------CORROTINAS--------------------

    #region --------------------GIZMOS--------------------
    private void OnDrawGizmos()
    {
        // Desenha a esfera de colisão quando o objeto está selecionado no Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, obstacleDetectionRadius);
    }
    #endregion --------------------GIZMOS--------------------
}
