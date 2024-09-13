using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] public GameObject _target;

    [Header("MOVEMENT")]
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _rotateSpeed = 95f;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100f;
    [SerializeField] private float _minDistancePredict = 5f;
    [SerializeField] private float _maxTimePrediction = 5f;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50f;
    [SerializeField] private float _deviationSpeed = 2f;

    private Rigidbody _targetRb;

    private void Start()
    {
        if (_target != null)
        {
            Debug.Log("Missile targeting: " + _target.name + " at position: " + _target.transform.position);
            _targetRb = _target.GetComponent<Rigidbody>();

            if (_targetRb == null)
            {
                Debug.LogWarning("Target does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Missile has no target assigned at the start.");
        }
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            Debug.LogError("Missile has lost its target.");
            return;
        }

        // Update target and missile positions for debugging
        Debug.Log("Target position: " + _target.transform.position);
        Debug.Log("Target velocity: " + _targetRb.velocity);

        // Calculate the lead time percentage
        float leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));
        Debug.Log("Lead time percentage: " + leadTimePercentage);

        // Perform prediction and deviation
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);

        // Move and rotate the missile
        _rb.velocity = transform.forward * _speed;
        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        if (_targetRb == null) return;

        float predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
        _standardPrediction = _targetRb.position + _targetRb.velocity * predictionTime;

        // Log the standard prediction
        Debug.Log("Predicted position (standard): " + _standardPrediction);
    }

    private void AddDeviation(float leadTimePercentage)
    {
        if (_target == null) return;

        // Calculate deviation
        Vector3 deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, Mathf.Sin(Time.time * _deviationSpeed));
        Vector3 predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standardPrediction + predictionOffset;

        // Log the deviated prediction
        Debug.Log("Predicted position (with deviation): " + _deviatedPrediction);
    }

    private void RotateRocket()
    {
        if (_target == null) return;

        Vector3 heading = _deviatedPrediction - transform.position;
        Quaternion rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));

        // Log the missile's rotation
        Debug.Log("Missile rotation: " + transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the missile on collision
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _standardPrediction);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
        }
    }
}
