using System.Collections;
using UnityEngine;

namespace Infrastructure.Service
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
    }
}