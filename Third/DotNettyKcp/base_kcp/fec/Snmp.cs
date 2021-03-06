namespace fec;

public class Snmp
{
    public static volatile Snmp snmp = new();

    // accumulated active open connections
    public int ActiveOpens;

    // bytes received to upper level
    public int BytesReceived;

    // bytes sent from upper level
    public int BytesSent;

    // current number of established connections
    public int CurrEstab;

    // accmulated early retransmitted segments
    public int EarlyRetransSegs;

    // accmulated fast retransmitted segments
    public int FastRetransSegs;

    // 收到的 Data数量
    public int FECDataShards;

    // incorrect packets recovered from FEC
    public int FECErrs;

    // 收到的 Parity数量
    public int FECParityShards;

    // correct packets recovered from FEC
    public int FECRecovered;

    // number of data shards that's not enough for recovery
    public int FECRepeatDataShards;

    // number of data shards that's not enough for recovery
    public int FECShortShards;

    // UDP bytes received
    public int InBytes;

    // checksum errors from CRC32
    public int InCsumErrors;

    // UDP read errors reported from net.PacketConn
    public int InErrs;

    // incoming packets count
    public int InPkts;

    // incoming KCP segments
    public int InSegs;

    // packet iput errors reported from KCP
    public int KCPInErrors;

    // number of segs infered as lost
    public int LostSegs;

    // max number of connections ever reached
    public int MaxConn;

    // UDP bytes sent
    public int OutBytes;

    // outgoing packets count
    public int OutPkts;

    // outgoing KCP segments
    public int OutSegs;

    // accumulated passive open connections
    public int PassiveOpens;

    // number of segs duplicated
    public int RepeatSegs;

    // accmulated retransmited segments
    public int RetransSegs;

    public override string ToString()
    {
        return "Snmp{" +
               "BytesSent=" + BytesSent +
               ", BytesReceived=" + BytesReceived +
               ", MaxConn=" + MaxConn +
               ", ActiveOpens=" + ActiveOpens +
               ", PassiveOpens=" + PassiveOpens +
               ", CurrEstab=" + CurrEstab +
               ", InErrs=" + InErrs +
               ", InCsumErrors=" + InCsumErrors +
               ", KCPInErrors=" + KCPInErrors +
               ", 收到包=" + InPkts +
               ", 发送包=" + OutPkts +
               ", InSegs=" + InSegs +
               ", OutSegs=" + OutSegs +
               ", 收到字节=" + InBytes +
               ", 发送字节=" + OutBytes +
               ", 总共重发数=" + RetransSegs +
               ", 快速重发数=" + FastRetransSegs +
               ", 空闲快速重发数=" + EarlyRetransSegs +
               ", 超时重发数=" + LostSegs +
               ", 收到重复包数量=" + RepeatSegs +
               ", fec恢复数=" + FECRecovered +
               ", fec恢复错误数=" + FECErrs +
               ", 收到fecData数=" + FECDataShards +
               ", 收到fecParity数=" + FECParityShards +
               ", fec缓存冗余淘汰data包数=" + FECShortShards +
               ", fec收到重复的数据包=" + FECRepeatDataShards +
               '}';
    }
}