declare @search varchar(max);
set @search = '1234';

select * from sales.tblCustomers
WHERE customerFirstName like @search + '%' 
    OR customerLastName like @search + '%' 
    OR customerEmail like @search + '%' 
    --OR customerPhone like @search + '%' 
    OR Cast(customerId as varchar) like @search + '%' 
