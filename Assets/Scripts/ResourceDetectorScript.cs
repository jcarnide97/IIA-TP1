using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 0.1f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angle;
    public int numObjects;
    public bool debug_mode;
    // Limiares e limites - alterar se necessário
    public float limiarInferior = 0.25f;
    public float limiarSuperior = 0.75f;
    public float limiteInferior = 0.05f;
    public float limiteSuperior = 0.6f;
    // Média e desvio padrão variáveis
    public float media = 0.5f;
    public float desvio = 0.12f;

    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // FixedUpdate is called at fixed intervals of time
    void FixedUpdate()
    {
        ObjectInfo anObject;
        anObject = GetClosestPickup();
        if (anObject != null)
        {
            angle = anObject.angle;
            strength = 1.0f / (anObject.distance + 1.0f);
        }
        else
        { // no object detected
            strength = 0;
            angle = 0;
        }

    }

    public float GetAngleToClosestResource()
    {
        return angle;
    }


    public float GetLinearOutput()
    {
        float energia = strength;

        if (strength == 0.0f)
        {
            return 0.0f;
        }
        else if (strength < limiarInferior)
        {
            return limiteInferior;
        }
        else if (strength > limiarSuperior)
        {
            return limiteInferior;
        }
        else if (energia < limiteInferior)
        {
            return limiteInferior;
        }
        else if (energia > limiteSuperior)
        {
            return energia;
        }
        return energia;
    }

    public virtual float GetGaussianOutput()
    {
        float energia = (1 / (desvio * (float)Math.Sqrt(2 * (float)Math.PI))) * (float)Math.Exp(-0.5f * (float)Math.Pow((strength - media) / desvio, 2));

        if (strength == 0.0f)
        {
            return 0.0f;
        }
        else if (strength < limiarInferior)
        {
            return limiteInferior;
        }
        else if (strength > limiarSuperior)
        {
            return limiteInferior;
        }
        else if (energia < limiteInferior)
        {
            return limiteInferior;
        }
        else if (energia > limiteSuperior)
        {
            return energia;
        }
        return energia;
    }

    public virtual float GetLogaritmicOutput()
    {
        float energia = - (float)Math.Log(this.strength);

        if (strength == 0.0f)
        {
            return 0.0f;
        }
        else if (strength < limiarInferior)
        {
            return limiteInferior;
        }
        else if (strength > limiarSuperior)
        {
            return limiteInferior;
        }
        else if (energia < limiteInferior)
        {
            return limiteInferior;
        }
        else if (energia > limiteSuperior)
        {
            return energia;
        }
        return energia;
    }


    public ObjectInfo[] GetVisiblePickups()
    {
        return (ObjectInfo[]) GetVisibleObjects("Pickup").ToArray();
    }

    public ObjectInfo GetClosestPickup()
    {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Pickup").ToArray();
        if(a.Length == 0)
        {
            return null;
        }
        return a[a.Length-1];
    }

    public List<ObjectInfo> GetVisibleObjects(string objectTag)
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        for (int i = 0; i * angleOfSensors < 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debug_mode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }


    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);

    }
}
