namespace HighSpeedCircularBuffer

module CircularBuffers =

    open System
    open System.Runtime.CompilerServices

    [<IsByRefLike; Struct>]
    type private StructArray = 

        val arr: float[]

        new(size:int) =
            { arr = Array.create size nan }



    type CircularBuffer(size:int) =

        let mutable lastTick = nan
    
        let structArray = StructArray(size * 2)

        let arr = structArray.arr      

        let mutable headIndex = size - 1

        member x.Push value =

            if headIndex + 1 = size * 2 then
                headIndex <- size
            else
                headIndex <- headIndex + 1

            let span = new Span<float>(arr)

            span.[headIndex] <- value

            span.[headIndex - size] <- value

            lastTick <- value
        

        member x.LastTick with get() = lastTick


        member x.LastNticks with get(nTicks) =
    
            if nTicks > size then
        
                failwith "Number of requested ticks exceeds number of ticks"

            else

                let startIndex = headIndex - nTicks + 1

                let span = new Span<float>(arr)

                span.Slice(startIndex, nTicks).ToArray()


        member x.GetTick(nTicksPrior) =

            if nTicksPrior > size then
                
                failwith "Number of requested ticks exceeds number of ticks"

            else 
                
                let span = new Span<float>(arr)

                span.[headIndex - nTicksPrior]


        member x.PrintState() =
    
            arr
            |> Array.iteri(fun i x ->
        
                sprintf "Pos %i: %f" i x
                |> Console.WriteLine
        
                )


        member x.Reset() =
    
            for i in 0..(size * 2 - 1) do
                arr.[i] <- (nan)

            lastTick <- nan

            headIndex <- size - 1


        member x.HeadIndex with get() = headIndex



    type CircularBufferExtlSpan(size:int) =

        let mutable lastTick = nan
    
        let structArray = StructArray(size * 2)

        let arr = structArray.arr      

        let mutable headIndex = size - 1

        member x.Span with get() = Span<float>(arr)

        member x.Push (value, span:Span<float>) =

            if headIndex + 1 = size * 2 then
                headIndex <- size
            else
                headIndex <- headIndex + 1

            span.[headIndex] <- value

            span.[headIndex - size] <- value

            lastTick <- value
        

        member x.LastTick with get() = lastTick


        member x.LastNticks with get(nTicks, span:Span<float>) =
    
            if nTicks > size then
        
                failwith "Number of requested ticks exceeds number of ticks"

            else

                let startIndex = headIndex - nTicks + 1

                span.Slice(startIndex, nTicks).ToArray()


        member x.GetTick(nTicksPrior, span:Span<float>) =

            if nTicksPrior > size then
                
                failwith "Number of requested ticks exceeds number of ticks"

            else 

                span.[headIndex - nTicksPrior]


        member x.PrintState() =
    
            arr
            |> Array.iteri(fun i x ->
        
                sprintf "Pos %i: %f" i x
                |> Console.WriteLine
        
                )


        member x.Reset() =
    
            for i in 0..(size * 2 - 1) do
                arr.[i] <- (nan)

            lastTick <- nan

            headIndex <- size - 1


        member x.HeadIndex with get() = headIndex


