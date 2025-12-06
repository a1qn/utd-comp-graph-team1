using System.Collections;
using UnityEngine;

public class StudentCross : MonoBehaviour
{
    [SerializeField] GameObject students;
    [SerializeField] float moveTime = 5f;
    [SerializeField] float moveDistance = 10f;
    string playerString = "Player";
    bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
       Debug.Log(other.tag);
        if (other.CompareTag(playerString) && !isTriggered)
        {
            isTriggered = true;
            //Debug.Log("is triggered");
            // Activate students
            students.SetActive(true);

            // Start moving them
            StartCoroutine(MoveStudentsForward());
        }
    }

    IEnumerator MoveStudentsForward()
    {
        float elapsed = 0f;

        // Starting & target position
        Vector3 startPos = students.transform.position;
        Vector3 targetPos = startPos + students.transform.forward * moveDistance; // move 3 meters forward

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;

            // Lerp movement
            students.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveTime);

            yield return null;
        }

        // Snap to final position (optional)
        students.transform.position = targetPos;
        Destroy(students);
    }
}
