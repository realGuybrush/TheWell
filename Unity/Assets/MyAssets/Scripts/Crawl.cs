using UnityEngine;

public class BasicCrawl
{
    public bool crawling;
    public float crawlingMultiplier = 0.5f;
    public float rollSpeedX = 10.0f;
    public float rollSpeedY = 0.0f;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newThisObject)
    {
        thisObject = newThisObject;
    }

    public void Crawl()
    {
        crawling = true;
    }

    public void UnCrawl()
    {
        crawling = false;
    }

    public bool CheckCrawl()
    {
        return crawling;
    }
}