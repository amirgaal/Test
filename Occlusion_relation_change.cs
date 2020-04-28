 private void ac()
    {
        GameObject ga;
        for (int i = 0; i < childs.Length - 1; i++)
        {
            for (int j = 0; j < childs.Length - 1; j++)
            {
                if (childs[j].GetComponent<lookCamera>().Distance() < childs[j + 1].GetComponent<lookCamera>().Distance())
                {
                    ga = childs[j];
                    childs[j] = childs[j + 1];
                    childs[j + 1] = ga;
                }
            }
        }
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].transform.SetSiblingIndex(i);
        }
    }
