--- 3 numbers from console , check if the numbers are "mountain shape" and a and c not equal: a < b > c ---

start
	declare
        var number1 integer;
        var number2 integer;
        var number3 integer;
        var valid bool;
    end-declare
    body
        read number1;
        read number2;
        read number3;

        valid is true;

        if ((number1 > number2) or (number2 < number3))
            valid is false;
        endif

        if (number1 = number2)
            valid is false;
        endif
        
        print valid;
        
        write product;
    end-body
end