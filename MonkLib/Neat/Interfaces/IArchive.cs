using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat.Interfaces
{
    public interface IArchive
    {
        void CreateRecord(ConnectionGene connectionData, NodeGene nodeData = null);
        object GetRecord(object key);
        bool HasRecord(object key);
        bool HasActiveRecord(object key);
    }
}
