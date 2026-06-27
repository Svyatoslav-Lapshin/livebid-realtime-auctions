CREATE TABLE users (
	id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	display_name varchar(100) NOT NULL,
	email varchar(255) NOT NULL UNIQUE ,
	password_hash varchar(255) NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz

)