--- product of 2 numbers , read and write result to console ---

start
	declare
        var number1 integer;
        var number2 integer;
        var product integer;
    end-declare
    body
        read number1;
        read number2;

        product is number1 * number2;
        
        write product;
    end-body
end