CREATE FUNCTION incrementBusinessAndUserLikeCount() 
RETURNS trigger AS
$$
BEGIN
	UPDATE Business
	SET numTips = (numTips + 1)
	WHERE Business.business_id = NEW.business_id;
	UPDATE _User
	SET total_tip_count = (total_tip_count + 1)
	WHERE _User.user_id = NEW.user_id;
	RETURN NEW;
END
$$ LANGUAGE plpgsql;
