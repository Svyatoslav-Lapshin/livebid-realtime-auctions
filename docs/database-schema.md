\# LiveBid Database Schema



\## users



\### Purpose



Stores registered users of the LiveBid platform.



\### Columns



| Column | Type | Required | Description |

|---|---|---|---|

| id | uuid | yes | Primary key |

| display\_name | varchar(100) | yes | Public user name |

| email | varchar(255) | yes | User email address |

| password\_hash | varchar(255) | yes | Hashed user password |

| created\_at | timestamptz | yes | Date and time when user was created |

| updated\_at | timestamptz | no | Date and time when user was last updated |



\### Primary Key



id



\### Foreign Keys



None



\### Indexes



email should be unique.



\---



\## auctions



\### Purpose



Stores auction listings created by users.



\### Columns



| Column | Type | Required | Description |

|---|---|---|---|

| id | uuid | yes | Primary key |

| seller\_id | uuid | yes | User who created the auction |

| title | varchar(200) | yes | Auction title |

| description | text | yes | Auction description |

| start\_price | numeric(18,2) | yes | Starting price |

| current\_price | numeric(18,2) | yes | Current highest price |

| start\_time | timestamptz | yes | Date and time when auction starts |

| end\_time | timestamptz | yes | Date and time when auction ends |

| status | varchar(30) | yes | Auction status: draft, scheduled, live, ended, cancelled |

| created\_at | timestamptz | yes | Date and time when auction was created |

| updated\_at | timestamptz | no | Date and time when auction was last updated |



\### Primary Key



id



\### Foreign Keys



seller\_id references users(id)



\### Indexes



seller\_id should be indexed.



status should be indexed.



end\_time should be indexed.



\---



\## bids



\### Purpose



Stores bids placed by users on auctions.



\### Columns



| Column | Type | Required | Description |

|---|---|---|---|

| id | uuid | yes | Primary key |

| auction\_id | uuid | yes | Auction where the bid was placed |

| bidder\_id | uuid | yes | User who placed the bid |

| amount | numeric(18,2) | yes | Bid amount |

| placed\_at | timestamptz | yes | Date and time when bid was placed |



\### Primary Key



id



\### Foreign Keys



auction\_id references auctions(id)



bidder\_id references users(id)



\### Indexes



auction\_id should be indexed.



bidder\_id should be indexed.



auction\_id and placed\_at should be indexed together.



\---



\## auction\_events



\### Purpose



Stores the history of important auction events.



Examples:



\- auction created

\- auction started

\- bid placed

\- auction ended

\- auction cancelled



\### Columns



| Column | Type | Required | Description |

|---|---|---|---|

| id | uuid | yes | Primary key |

| auction\_id | uuid | yes | Auction related to this event |

| event\_type | varchar(50) | yes | Type of auction event |

| actor\_user\_id | uuid | no | User who caused the event |

| metadata | jsonb | no | Additional event data |

| occurred\_at | timestamptz | yes | Date and time when event happened |



\### Primary Key



id



\### Foreign Keys



auction\_id references auctions(id)



actor\_user\_id references users(id)



\### Indexes



auction\_id should be indexed.



event\_type should be indexed.



auction\_id and occurred\_at should be indexed together.



\---



\## notifications



\### Purpose



Stores user notifications.



Examples:



\- user was outbid

\- user won an auction

\- auction ended

\- bid was placed on user's auction



\### Columns



| Column | Type | Required | Description |

|---|---|---|---|

| id | uuid | yes | Primary key |

| user\_id | uuid | yes | User who receives the notification |

| title | varchar(200) | yes | Notification title |

| message | text | yes | Notification message |

| is\_read | boolean | yes | Shows whether notification was read |

| created\_at | timestamptz | yes | Date and time when notification was created |



\### Primary Key



id



\### Foreign Keys



user\_id references users(id)



\### Indexes



user\_id should be indexed.



is\_read should be indexed.



user\_id and created\_at should be indexed together.

