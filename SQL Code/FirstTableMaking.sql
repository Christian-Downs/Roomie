use Roomie;
create table SingleMessage(
	id int primary key not null,
	TimeOfMessage DateTime,
	MessageText NVARCHAR(MAX)
);

create table [Address] (
	ID int primary key not null,
	City NVarchar(max),
	Country NVARCHAR(MAX),
	Street NVARCHAR(MAX)
);


create table Photo(
	ID int primary key not null,
	ImageLocation nvarchar(max) not null
);

create table Appartments(
	ID int primary key not null,
	RentCost money not null,
	[Description] nvarchar(max) not null,
);


create table ProfileLinker(
	ID int primary key not null,
	Liked bit,
	Favorited bit,
	AppartmentsID int references [Appartments](Id)
	);



create table UserProfile(
	Id nvarchar(128) primary key,
	FirstName nvarchar(26) NOT NULL,
	LastName nvarchar(26) not null,
	PhoneNumber nvarchar(20),
	EmailAddress nvarchar(256) NOT NULL,
	[Description] nvarchar(	max),
	PropertyBool BIT not null,
	AddressID int,
	ProfileLinkerId int,
	PhotoID int references photo(id),
	foreign key(addressId) references [Address](ID),
	foreign key(ProfileLinkerId) references ProfileLinker(ID),
	);

	create table [MessageBoard] (
	SenderID nvarchar(128) references UserProfile(id) not null,
	RecieverID nvarchar(128) references UserProfile(id) not null,
	ProfileLinkerID int references ProfileLinker(id) not null,
	MessageID int references SingleMessage(id)
	primary key(SenderID, RecieverID)
);

alter table appartments
add ProfileLinkerID int not null references ProfileLinker(id);

alter table profileLinker
add UserLinkedId nvarchar(128) not null references UserProfile(id),
	LinkedProfile nvarchar(128) not null references UserProfile(id);

alter table Appartments
	add PhotoID int references Photo(id);

alter table appartments
	drop constraint FK__Appartmen__Profi__49C3F6B7


alter table appartments
	drop column ProfileLinkerID;

alter table userprofile
	drop column 