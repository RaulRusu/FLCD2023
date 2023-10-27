--- product of 2 numbers , read and write result to console ---

start
	declare
        var number1 integer --- err ---
        var number2 integer;
        product integer; --- err ---
    end-declare
    body
        read number1;
        read number2;

        product = number1 * number2; --- no err ---
        
        write product;
    --- err no endbody ---
end