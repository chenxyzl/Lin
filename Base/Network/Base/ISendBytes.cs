﻿using System.Threading.Tasks;

namespace Base.Network;

/// <summary>
///     发送字节
/// </summary>
public interface ISendBytes
{
    /// <summary>
    ///     发送字节
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <returns></returns>
    Task Send(byte[] bytes);
}