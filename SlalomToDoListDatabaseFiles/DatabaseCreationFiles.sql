CREATE DATABASE  IF NOT EXISTS `slalomtodolist` ;
Create table slalomtodolist.users_tbl(
   user_id INT NOT NULL AUTO_INCREMENT,
   user_first_name VARCHAR(128) NOT NULL,
   user_last_name VARCHAR(128) NOT NULL,
   create_date datetime NOT NULL,
   modified_date datetime NOT NULL,
   PRIMARY KEY ( user_id )
);

insert into users_tbl (user_first_name, user_last_name, create_date, modified_date)
value
('Test', 'User', SYSDATE( ), SYSDATE( ));

create table slalomtodolist.list_tbl(
   list_id INT NOT NULL auto_increment,
   user_id INT NOT NULL,
   item varchar(1024) NOT NULL,
   create_date datetime NOT NULL,
   modified_date datetime NOT NULL,
   is_completed nvarchar(1) NOT NULL,
   completion_date datetime NOT NULL,
   primary key (list_id),
   foreign key(user_id) REFERENCES users_tbl(user_id)
   );

insert into list_tbl
(  user_id, 
   item, 
   create_date, 
   modified_date, 
   is_completed,
   completion_date
)
values
(1, 'Test Entry', sysdate(), sysdate(), 'N', sysdate()+30)