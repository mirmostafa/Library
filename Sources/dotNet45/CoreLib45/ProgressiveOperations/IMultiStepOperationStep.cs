#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.ProgressiveOperations
{
    public interface IMultiStepOperationStep
    {
        Action<MultiStepOperation> Step { get; set; }
        MultiStepOperation Operation { get; }
        string Description { get; }
        int PriorityId { get; }
        long Id { get; }
    }
}