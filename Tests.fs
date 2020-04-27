namespace HighSpeedCircularBuffer

module Tests =

    open NUnit.Framework
    open FsUnit

    open HighSpeedCircularBuffer.CircularBuffers

    [<Test>]
    let ``Regular CB: Add 3 ticks, return 3 ticks``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        let data = cb.LastNticks 3

        data |> should equal [| 1.0; 2.0; 3.0 |]


    [<Test>]
    let ``Regular CB: Add 3 ticks, return 10 ticks``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        let data = cb.LastNticks 10

        data |> should equal [| nan; nan; nan; nan; nan; nan; nan; 1.0; 2.0; 3.0 |]       


    [<Test>]
    let ``Regular CB: Add 3 ticks, fail to return 11 ticks``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        try
            cb.LastNticks 11 |> ignore
            failwith "failed"            
        with
        | ex ->        
            ex.Message 
            |> should equal
                "Number of requested ticks exceeds number of ticks"


    [<Test>]
    let ``Regular CB: Add 11 ticks, return 3 ticks``() =

        let cb = CircularBuffer(10)

        for i in 1..11 do
            (float)i |> cb.Push

        let data = cb.LastNticks 3

        data |> should equal [| 9.0; 10.0; 11.0 |]

    [<Test>]
    let ``Regular CB: Add 11 ticks, fail to return 11 ticks``() =

        let cb = CircularBuffer(10)

        for i in 1..11 do
            (float)i |> cb.Push

        try
            cb.LastNticks 11 |> ignore
            failwith "failed"            
        with
        | ex ->        
            ex.Message 
            |> should equal
                "Number of requested ticks exceeds number of ticks"

    [<Test>]
    let ``Regular CB: Add 3 ticks, return last tick``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        let tick = cb.LastTick

        tick |> should equal 3.0


    [<Test>]
    let ``Regular CB: Add 11 ticks, return last tick``() =

        let cb = CircularBuffer(10)

        for i in 1..11 do
            (float)i |> cb.Push

        let tick = cb.LastTick

        tick |> should equal 11.0


    [<Test>]
    let ``Regular CB: Add 3 ticks, return 2 ticks prior``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        let tick = cb.GetTick 2

        tick |> should equal 1.0

    [<Test>]
    let ``Regular CB: Add 3 ticks, return 3 ticks prior``() =

        let cb = CircularBuffer(10)

        cb.Push 1.0
        cb.Push 2.0
        cb.Push 3.0

        let tick = cb.GetTick 3

        tick |> should equal nan


    [<Test>]
    let ``Regular CB: Add 11 ticks, return 3 ticks prior``() =

        let cb = CircularBuffer(10)

        for i in 1..11 do
            (float)i |> cb.Push

        let tick = cb.GetTick 3

        tick |> should equal 8.0




    [<Test>]
    let ``Extl Span CB: Add 3 ticks, return 3 ticks``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        let data = cb.LastNticks(3, span)

        data |> should equal [| 1.0; 2.0; 3.0 |]


    [<Test>]
    let ``Extl Span CB: Add 3 ticks, return 10 ticks``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        let data = cb.LastNticks(10, span)

        data |> should equal [| nan; nan; nan; nan; nan; nan; nan; 1.0; 2.0; 3.0 |]       


    [<Test>]
    let ``Extl Span CB: Add 3 ticks, fail to return 11 ticks``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        try
            cb.LastNticks(11, span) |> ignore
            failwith "failed"            
        with
        | ex ->        
            ex.Message 
            |> should equal
                "Number of requested ticks exceeds number of ticks"


    [<Test>]
    let ``Extl Span CB: Add 11 ticks, return 3 ticks``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        for i in 1..11 do
            cb.Push(float(i), span)

        let data = cb.LastNticks(3, span)

        data |> should equal [| 9.0; 10.0; 11.0 |]

    [<Test>]
    let ``Extl Span CB: Add 11 ticks, fail to return 11 ticks``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        for i in 1..11 do
            cb.Push(float(i), span)

        try
            cb.LastNticks(11, span) |> ignore
            failwith "failed"            
        with
        | ex ->        
            ex.Message 
            |> should equal
                "Number of requested ticks exceeds number of ticks"

    [<Test>]
    let ``Extl Span CB: Add 3 ticks, return last tick``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        let tick = cb.LastTick

        tick |> should equal 3.0


    [<Test>]
    let ``Extl Span CB: Add 11 ticks, return last tick``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        for i in 1..11 do
            cb.Push(float(i), span)

        let tick = cb.LastTick

        tick |> should equal 11.0


    [<Test>]
    let ``Extl Span CB: Add 3 ticks, return 2 ticks prior``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        let tick = cb.GetTick(2, span)

        tick |> should equal 1.0

    [<Test>]
    let ``Extl Span CB: Add 3 ticks, return 3 ticks prior``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        cb.Push(1.0, span)
        cb.Push (2.0, span)
        cb.Push (3.0, span)

        let tick = cb.GetTick(3, span)

        tick |> should equal nan


    [<Test>]
    let ``Extl Span CB: Add 11 ticks, return 3 ticks prior``() =

        let cb = CircularBufferExtlSpan(10)
        let span = cb.Span

        for i in 1..11 do
            cb.Push(float(i), span)

        let tick = cb.GetTick(3, span)

        tick |> should equal 8.0