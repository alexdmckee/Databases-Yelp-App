CREATE OR REPLACE FUNCTION distance(userLatitude float8, userLongitude float8, businessLatitude float8, businessLongitude float8) RETURNS float8 AS $dist$
	DECLARE
        dlat float8;
        dlong float8;
        a float8;
        c float8;
		R float8;
		dist float8= 0;
	BEGIN
		dlat = userLatitude - businessLatitude;
		dlong = userLongitude - businessLongitude;
		a = (sin(dlat/2)*sin(dlat/2))+cos(userLatitude)*cos(businessLatitude)*(sin(dlong/2)*sin(dlong/2));
		c = 2 * atan2(sqrt(a), sqrt(1-a));
		R = 3958.8;
		dist = R * c;
		return dist;
	END;
$dist$ LANGUAGE plpgsql;