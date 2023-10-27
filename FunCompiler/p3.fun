--- print to console the max of an array of numbers, the lenght of an array is always 10 ---

start
	declare
        var maxNumber integer;
        var list array of integer 50;
        const n integer is 10;
        var i of integer;
    end-declare
    body
        for i (0,n,1)
            read list[i];
        endfor

        for i (0,n,1)
            if maxNumber < list[i]
                maxNumber is list[i]
            endif
        endfor
        
        write maxNumber;
    end-body
end