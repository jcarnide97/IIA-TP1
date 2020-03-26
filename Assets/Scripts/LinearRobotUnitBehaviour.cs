using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float weightBlock;

    public float resourceValue;
    public float resouceAngle;

    public float blockValue;
    public float blockAngle;

    void Update()
    {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        blockAngle = blockDetector.GetAngleToClosestObstacle();

        //SEM USAR weight
        // Linear
        //resourceValue = resourcesDetector.GetLinearOutput();
        //blockValue = blockDetector.GetLinearOutput();
        //Gaussiana
        //resourceValue = resourcesDetector.GetGaussianOutput();
        //blockValue = blockDetector.GetGaussianOutput();
        //Logaritmo Negativo
        resourceValue = resourcesDetector.GetLogaritmicOutput();
        //blockValue = blockDetector.GetLogaritmicOutput();

        //USANDO weight
        // Linear
        //resourceValue = weightResource * resourcesDetector.GetLinearOutput();
        //blockValue = weightBlock * blockDetector.GetLinearOutput();
        //Gaussiana
        //resourceValue = weightResource * resourcesDetector.GetGaussianOutput();
        //blockValue = weightBlock * blockDetector.GetGaussianOutput();
        //Logaritmo Negativo
        //resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput();
        //blockValue = weightBlock * blockDetector.GetLogaritmicOutput();

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards
        applyForce(180 + blockAngle, blockValue);

    }


}
