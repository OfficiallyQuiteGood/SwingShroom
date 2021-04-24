﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour
{
    // Variable
    public Vector2 endPoint;
    public float speed = 1;
    public float distance = 2;
    public GameObject nodePrefab;
    private GameObject player;
    private GameObject lastNode;
    private List<GameObject> Nodes = new List<GameObject>();
    int vertexCount = 2;
    bool done = false;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Find player
        player = GameObject.FindGameObjectWithTag("Player");
        lastNode = transform.gameObject;
        Nodes.Add(transform.gameObject);
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPoint, speed);

        if ((Vector2)transform.position != endPoint)
        {
            float dist = Vector2.Distance(player.transform.position, lastNode.transform.position);
            if (dist > distance)
            {
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;
            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }

        RenderLine();
    }
    
    private void RenderLine()
    {
        lineRenderer.positionCount = vertexCount;
        int i;
        for (i = 0; i < Nodes.Count; i++)
        {
            lineRenderer.SetPosition(i, Nodes[i].transform.position);
        }
        lineRenderer.SetPosition(i, player.transform.position);
    }

    private void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)lastNode.transform.position;

        GameObject go = (GameObject) Instantiate(nodePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
    
        lastNode = go;

        Nodes.Add(lastNode);
        
        vertexCount++;
    }

    public void ClearRope()
    {
        foreach(GameObject go in Nodes)
        {
            Destroy(go);
        }
        Nodes.Clear();
        lineRenderer.positionCount = 0;
    }
}
