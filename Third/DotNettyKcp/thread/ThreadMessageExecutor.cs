using base_kcp;

namespace dotNetty_kcp.thread;

/**
     * 多生产者单消费者  560万  tps
     */
public class ThreadMessageExecutor : AbstratcMessageExecutor
{
    private const int MAX_QUEUE_SIZE = 2 << 10;

    private MpscArrayQueue<ITask> _queue;


    /**
 * 启动消息处理器
 */
    public override void start()
    {
        _queue = new MpscArrayQueue<ITask>(MAX_QUEUE_SIZE);
        base.start();
    }


    /**
         * 判断队列是否已经达到上限了
         * @return
         */
    public override bool isFull()
    {
        return _queue.Count == MAX_QUEUE_SIZE;
    }

    protected override bool isEmpty()
    {
        return _queue.IsEmpty;
    }

    protected override bool TryDequeue(out ITask task)
    {
        return _queue.TryDequeue(out task);
    }

    protected override bool TryEnqueue(ITask task)
    {
        return _queue.TryEnqueue(task);
    }
}