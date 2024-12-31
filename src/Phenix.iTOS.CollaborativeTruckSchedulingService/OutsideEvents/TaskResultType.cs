﻿namespace Phenix.iTOS.CollaborativeTruckSchedulingService.OutsideEvents;

/// <summary>
/// 任务结果类型
/// </summary>
public enum TaskResultType
{
    /// <summary>
    /// 完成
    /// </summary>
    Complete,

    /// <summary>
    /// 异常完成
    /// </summary>
    ManualComplete,

    /// <summary>
    /// 中止
    /// </summary>
    Abort,
}