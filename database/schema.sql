CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE users (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    display_name varchar(100) NOT NULL,
    email varchar(255) NOT NULL UNIQUE,
    password_hash varchar(255) NOT NULL,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz
);

CREATE TABLE auctions (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    seller_id uuid NOT NULL,
    title varchar(200) NOT NULL,
    description text NOT NULL,
    start_price numeric(18,2) NOT NULL,
    current_price numeric(18,2) NOT NULL,
    start_time timestamptz NOT NULL,
    end_time timestamptz NOT NULL,
    status varchar(30) NOT NULL,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,

    CONSTRAINT fk_auctions_seller
        FOREIGN KEY (seller_id)
        REFERENCES users(id),

    CONSTRAINT chk_auctions_start_price_positive
        CHECK (start_price >= 0),

    CONSTRAINT chk_auctions_current_price_positive
        CHECK (current_price >= 0),

    CONSTRAINT chk_auctions_time_order
        CHECK (end_time > start_time),

    CONSTRAINT chk_auctions_status
        CHECK (status IN ('draft', 'scheduled', 'live', 'ended', 'cancelled'))
);

CREATE TABLE bids (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    auction_id uuid NOT NULL,
    bidder_id uuid NOT NULL,
    amount numeric(18,2) NOT NULL,
    placed_at timestamptz NOT NULL DEFAULT now(),
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,

    CONSTRAINT fk_bids_auction
        FOREIGN KEY (auction_id)
        REFERENCES auctions(id),

    CONSTRAINT fk_bids_bidder
        FOREIGN KEY (bidder_id)
        REFERENCES users(id),

    CONSTRAINT chk_bids_amount_positive
        CHECK (amount > 0)
);

CREATE TABLE auction_events (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    auction_id uuid NOT NULL,
    event_type varchar(50) NOT NULL,
    actor_user_id uuid,
    metadata jsonb,
    occurred_at timestamptz NOT NULL DEFAULT now(),

    CONSTRAINT fk_auction_events_auction
        FOREIGN KEY (auction_id)
        REFERENCES auctions(id),

    CONSTRAINT fk_auction_events_actor_user
        FOREIGN KEY (actor_user_id)
        REFERENCES users(id),

    CONSTRAINT chk_auction_events_event_type
        CHECK (event_type IN (
            'auction_created',
            'auction_started',
            'bid_placed',
            'auction_ended',
            'auction_cancelled'
        ))
);

CREATE TABLE notifications (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id uuid NOT NULL,
    title varchar(200) NOT NULL,
    message text NOT NULL,
    is_read boolean NOT NULL DEFAULT false,
    created_at timestamptz NOT NULL DEFAULT now(),

    CONSTRAINT fk_notifications_user
        FOREIGN KEY (user_id)
        REFERENCES users(id)
);

CREATE INDEX idx_auctions_seller_id
ON auctions(seller_id);

CREATE INDEX idx_auctions_status
ON auctions(status);

CREATE INDEX idx_auctions_end_time
ON auctions(end_time);

CREATE INDEX idx_bids_auction_id
ON bids(auction_id);

CREATE INDEX idx_bids_bidder_id
ON bids(bidder_id);

CREATE INDEX idx_bids_auction_id_placed_at
ON bids(auction_id, placed_at);

CREATE INDEX idx_auction_events_auction_id
ON auction_events(auction_id);

CREATE INDEX idx_auction_events_event_type
ON auction_events(event_type);

CREATE INDEX idx_auction_events_auction_id_occurred_at
ON auction_events(auction_id, occurred_at);

CREATE INDEX idx_notifications_user_id
ON notifications(user_id);

CREATE INDEX idx_notifications_is_read
ON notifications(is_read);

CREATE INDEX idx_notifications_user_id_created_at
ON notifications(user_id, created_at);