using System;
using System.Linq;
using System.Collections.Generic;

namespace MonkLib.Neat
{
    public struct ArchiveRecord
    {
        public readonly ConnectionGene connection;
        public readonly NodeGene wasSplitBy;
        
        public ArchiveRecord(ConnectionGene connection, NodeGene node)
        {
            this.connection = connection;
            this.wasSplitBy = node;
        }
    }

    public struct RecordIndex
    {
        public readonly uint inputID;
        public readonly uint outputID;

        public RecordIndex(uint inputID, uint outputID)
        {
            this.inputID = inputID;
            this.outputID = outputID;
        }

        public override int GetHashCode()
        {
            return (this.inputID ^ this.outputID).GetHashCode();
        }
    }

    //singleton class
    public sealed class GeneArchive
    {
        #region Properties

        private static Lazy<GeneArchive> instance = new Lazy<GeneArchive>(() => new GeneArchive());
        private Dictionary<RecordIndex, ArchiveRecord> records;
        public static GeneArchive Instance { get => instance.Value; }

        #endregion

        #region Constructors

        private GeneArchive()
        {
            this.records = new Dictionary<RecordIndex, ArchiveRecord>();
        }

        #endregion

        #region Utils

        public void CreateRecord(ConnectionGene connection, NodeGene wasSplitBy = null)
        {
            RecordIndex index = new RecordIndex(connection.Input.Innovation, connection.Output.Innovation);
            ArchiveRecord record = new ArchiveRecord(connection.Copy(), wasSplitBy != null ? wasSplitBy.Copy() : null);

            if (!this.HasRecord(index))
            {
                this.records.Add(index, record);
            }
            else
            {
                this.records[index] = record;
            }

        }

        public ArchiveRecord GetRecord(RecordIndex index)
        {
            if(this.HasRecord(index))
            {
                return new ArchiveRecord(this.records[index].connection, this.records[index].wasSplitBy);
            }

            return new ArchiveRecord(null, null);
        }

        public bool HasRecord(RecordIndex index)
        {
            return this.records.ContainsKey(index);
        }

        public bool HasActiveRecord(RecordIndex index)
        {
            if(this.records.ContainsKey(index))
            {
                return this.records[index].connection.Enabled;
            }

            return false;
        }
        #endregion

    }
}
